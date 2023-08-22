using GameSync.Api.Persistence.Entities;
using GameSync.Business.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace GameSync.Api.Endpoints.Users.IndividualUser;

public class ForgotPasswordEndpoint : Endpoint<SingleMailRequest, Results<Ok, StatusCodeHttpResult, BadRequestWhateverError>>
{
    private readonly UserManager<User> _manager;
    private readonly IForgotPasswordEmailSender _sender;

    public ForgotPasswordEndpoint(
        UserManager<User> manager, 
        IForgotPasswordEmailSender sender)
    {
        _manager = manager;
        _sender = sender;
    }

    public override void Configure()
    {
        Post("{Email}/forgot-password");
        Group<UsersGroup>();
    }

    public override async Task<Results<Ok, StatusCodeHttpResult, BadRequestWhateverError>> ExecuteAsync(SingleMailRequest req, CancellationToken ct)
    {
        var user = await _manager.FindByEmailAsync(req.Email);

        if (user is null) 
        {
            return TypedResults.Ok();
        }

        var token = await _manager.GeneratePasswordResetTokenAsync(user);

        if (!await _sender.SendForgotPasswordEmailAsync(req.Email, token))
        {
            return TypedResults.StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

        return TypedResults.Ok();
    }

}
