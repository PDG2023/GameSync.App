using FluentValidation;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using GameSync.Api.CommonRequests;
using GameSync.Api.Extensions;
using GameSync.Api.Resources;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace GameSync.Api.Endpoints.Users.PasswordReset;

public static class ChangePassword
{
    public class Request : RequestWithCredentials
    {
        public required string PasswordRepetition { get; init; }
        public required string Token { get; init; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            Include(new RequestWithCredentialsValidator());
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithResourceError(() => Resource.InvalidToken);

            RuleFor(x => x.PasswordRepetition)
                .NotEmpty()
                .Must((req, x) => req.Password == x)
                .WithResourceError(() => Resource.PasswordDontMatch);
        }
    }

    public class Endpoint : Endpoint<Request, Results<Ok, NotFound, BadRequestWhateverError>>
    {
        private readonly UserManager<User> _manager;

        public Endpoint(UserManager<User> manager)
        {
            _manager = manager;
        }

        public override void Configure()
        {
            Post("change-password");
            Group<PasswordResetGroup>();
        }

        public override async Task<Results<Ok, NotFound, BadRequestWhateverError>> ExecuteAsync(Request req, CancellationToken ct)
        {
            var user = await _manager.FindByEmailAsync(req.Email);

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(req.Token));
            var changePasswordResult = await _manager.ResetPasswordAsync(user, decodedToken, req.Password);

            if (!changePasswordResult.Succeeded)
            {
                return new BadRequestWhateverError(changePasswordResult.Errors);
            }

            return TypedResults.Ok();
        }
    }

}
