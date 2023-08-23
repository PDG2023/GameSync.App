using FluentValidation;
using GameSync.Api.Common;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Endpoints.Users;

public class SignInRequest : IRequestWithCredentials
{
    public required string Email { get; init; }

    public required string Password { get; init; }
}

public class SuccessfulSignInResponse
{
    public required string Email { get; init; }
    public required string Token { get; init; }
    public required string UserName { get; init; }
}

public class SignInRequestValidator : Validator<SignInRequest>
{
    public SignInRequestValidator() 
    {
        Include(new CredentialsValidator());
    }
}


public class SignInEndpoint : Endpoint<SignInRequest, Results<Ok<SuccessfulSignInResponse>,  BadRequestWhateverError>>
{
    private readonly SignInManager<User> signInManager;
    private readonly IConfiguration config;

    public SignInEndpoint(SignInManager<User> signInManager, IConfiguration config)
    {
        this.signInManager = signInManager;
        this.config = config;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Post("sign-in");
        Group<UsersGroup>();
       
    }

    public override async Task<Results<Ok<SuccessfulSignInResponse>, BadRequestWhateverError>> ExecuteAsync(SignInRequest req, CancellationToken ct)
    {
        
        var user = await signInManager.UserManager.FindByEmailAsync(req.Email);
        if (user is null)
        {
            AddNotFoundCredentialsErrors();
            return new BadRequestWhateverError(ValidationFailures);
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, req.Password, false);

        if (!signInResult.Succeeded)
        {
            if (signInResult.IsNotAllowed)
            {
                // TODO : Translate
                AddError(r => r.Email, "The email needs to be confirmed", "ConfirmationNeeded");
            }
            else
            {
                AddNotFoundCredentialsErrors();
            }
            return new BadRequestWhateverError(ValidationFailures);
        }


        var token = JWTBearer.CreateToken(
            signingKey: config["Jwt:SignKey"]!,
            issuer: config["Jwt:Issuer"]!,
            audience: config["Jwt:Issuer"]!,
            
            expireAt: DateTime.UtcNow.AddDays(1),
            priviledges: u => {
                u.Claims.Add((ClaimsTypes.UserId,  user.Id));
            });

        var response = new SuccessfulSignInResponse 
        { 
            Email = user.Email, 
            Token = token,
            UserName = user.UserName
        };
        return TypedResults.Ok(response);
    }

    private void AddNotFoundCredentialsErrors()
    {
        // TODO : Translate
        AddError(r => r.Email, "Nothing has been found for the given credentials", "NotFound");
    }
}
