using GameSync.Api.Persistence.Entities;
using GameSync.Business.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace GameSync.Api.Endpoints.Users;

public class SignUpRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class SuccessfulSignUpResponse
{
    public required string Email { get; set; }
}

public class SignUpEndpoint : Endpoint<SignUpRequest, Results<BadRequest<IEnumerable<IdentityError>>, StatusCodeHttpResult, Ok<SuccessfulSignUpResponse>>>
{
    private readonly UserManager<User> userManager;
    private readonly IAuthMailService authMailService;

    public SignUpEndpoint(UserManager<User> userManager, IAuthMailService authMailService)
    {
        this.userManager = userManager;
        this.authMailService = authMailService;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Post("sign-up");
        Group<UsersGroup>();
    }

    public override async Task<Results<BadRequest<IEnumerable<IdentityError>>, StatusCodeHttpResult, Ok<SuccessfulSignUpResponse>>> ExecuteAsync(SignUpRequest req, CancellationToken ct)
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

        var mailToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
        if (!await authMailService.SendEmailConfirmationAsync(newUser.Email, mailToken))
        {
            // delete the newly created user
            await userManager.DeleteAsync(newUser);

            // Send an error
            return TypedResults.StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

        return TypedResults.Ok(new SuccessfulSignUpResponse
        {
            Email = req.Email
        });
    }

}
