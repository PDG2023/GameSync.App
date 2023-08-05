using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Endpoints.Users;

public class SignInRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class SuccessfulSignInResponse
{
    public required string Email { get; init; }
}

public class SignInEndpoint : Endpoint<SignInRequest, Results<Ok<SuccessfulSignInResponse>, BadRequest<SignInResult>>>
{
    private readonly SignInManager<User> signInManager;

    public SignInEndpoint(SignInManager<User> signInManager)
    {
        this.signInManager = signInManager;
    }

    public override void Configure()
    {
        Post("sign-in");
        Group<UserAuthGroup>();
    }

    public override async Task<Results<Ok<SuccessfulSignInResponse>, BadRequest<SignInResult>>> ExecuteAsync(SignInRequest req, CancellationToken ct)
    {
        var user = new User
        {
            Email = req.Email,
            UserName = req.Email 
        };

        if (await signInManager.UserManager.CheckPasswordAsync(user, req.Password))
        {
            return TypedResults.BadRequest(SignInResult.Failed);
        }

        user = await signInManager.UserManager.FindByEmailAsync(req.Email);

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, req.Password, false);

        return signInResult.Succeeded 
            ? TypedResults.Ok(new SuccessfulSignInResponse { Email = req.Email }) 
            : TypedResults.BadRequest(signInResult);
    }



}
