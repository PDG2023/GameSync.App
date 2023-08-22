using GameSync.Api.Persistence.Entities;
using GameSync.Business.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace GameSync.Api.Endpoints.Users.IndividualUser;

public class ForgotPasswordEndpoint : Endpoint<SingleMailRequest, Results<Ok, StatusCodeHttpResult, BadRequestWhateverError>>
{
    private readonly UserManager<User> _manager;
    private readonly IPasswordResetMailSenderAsync _sender;

    public ForgotPasswordEndpoint(
        UserManager<User> manager, 
        IPasswordResetMailSenderAsync sender)
    {
        _manager = manager;
        _sender = sender;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Post("forgot-password");
        Group<UsersGroup>();
    }

    public override async Task<Results<Ok, StatusCodeHttpResult, BadRequestWhateverError>> ExecuteAsync(SingleMailRequest req, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            return new BadRequestWhateverError(ValidationFailures);
        }

        var user = await _manager.FindByEmailAsync(req.Email);

        if (user is null) 
        {
            return TypedResults.Ok();
        }

        var token = await _manager.GeneratePasswordResetTokenAsync(user);

        if (!await _sender.SendEmailPasswordResetAsync(req.Email, token))
        {
            return TypedResults.StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

        return TypedResults.Ok();
    }

}
