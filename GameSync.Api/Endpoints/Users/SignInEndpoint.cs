using Duende.IdentityServer.Configuration;
using FastEndpoints.Security;
using GameSync.Api.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Endpoints.Users;

public class SignInRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class SignInValidResponse
{
    public required string Email { get; set; }
}

[AllowAnonymous]
[HttpPost("users/sign-in")]
public class SignInEndpoint : Endpoint<SignInRequest, Results<BadRequest<IEnumerable<IdentityError>>, Ok<SignInValidResponse>>>
{
    private readonly UserManager<User> userManager;

    public SignInEndpoint(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    public override async Task<Results<BadRequest<IEnumerable<IdentityError>>, Ok<SignInValidResponse>>> ExecuteAsync(SignInRequest req, CancellationToken ct)
    {
        var newUser = new User
        {
            Email = req.Email,
            UserName = req.Email
        };

        var tryCreateUser = await userManager.CreateAsync(newUser, req.Password);

        if (!tryCreateUser.Succeeded)
        {
            return TypedResults.BadRequest(tryCreateUser.Errors);
        }

        return TypedResults.Ok(new SignInValidResponse
        {
            Email = req.Email
        });
    }
}
