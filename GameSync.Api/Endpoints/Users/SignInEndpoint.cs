using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GameSync.Api.Endpoints.Users;

public class SignInRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class SuccessfulSignInResponse
{
    public required string Email { get; init; }
    public required string Token { get; init; }
}

public class SignInEndpoint : Endpoint<SignInRequest, Results<Ok<SuccessfulSignInResponse>, BadRequest<SignInResult>>>
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

    public override async Task<Results<Ok<SuccessfulSignInResponse>, BadRequest<SignInResult>>> ExecuteAsync(SignInRequest req, CancellationToken ct)
    {
        
        var user = await signInManager.UserManager.FindByEmailAsync(req.Email);

        if (user is null)
        {
            return TypedResults.BadRequest(SignInResult.Failed);
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, req.Password, false);

        if (!signInResult.Succeeded)
        {
            return TypedResults.BadRequest(signInResult);
        }


        var token = JWTBearer.CreateToken(
            signingKey: config["Jwt:SignKey"],
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Issuer"],
            
            expireAt: DateTime.UtcNow.AddDays(1),
            priviledges: u => {
                u.Claims.Add(("userid", user.Id));
            });


        return TypedResults.Ok(new SuccessfulSignInResponse { Email = req.Email, Token = token });
    }



}
