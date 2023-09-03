using GameSync.Api.AuthMailServices;
using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;

namespace GameSync.Api.Endpoints.Users.PasswordReset;

public static class ForgotPassword
{
    public class Endpoint : Endpoint<RequestToUser, Results<Ok, StatusCodeHttpResult, BadRequestWhateverError>>
    {
        private readonly UserManager<User> _manager;
        private readonly IPasswordResetMailSender _sender;

        public Endpoint(
            UserManager<User> manager,
            IPasswordResetMailSender sender)
        {
            _manager = manager;
            _sender = sender;
        }

        public override void Configure()
        {
            Post("forgot-password");
            Group<PasswordResetGroup>();
        }

        public override async Task<Results<Ok, StatusCodeHttpResult, BadRequestWhateverError>> ExecuteAsync(RequestToUser req, CancellationToken ct)
        {
            var user = await _manager.FindByEmailAsync(req.Email);

            if (user is null)
            {
                return TypedResults.Ok();
            }

            var encodedToken = await _manager.GeneratePasswordResetTokenAsync(user);

            if (!await _sender.SendEmailPasswordResetAsync(req.Email, encodedToken))
            {
                return TypedResults.StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }

            return TypedResults.Ok();
        }

    }

}

