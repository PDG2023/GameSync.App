using FluentValidation;
using GameSync.Api.Extensions;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace GameSync.Api.Endpoints.Users;

public static class Confirm
{

    public class Request
    {
        public required string ConfirmationToken { get; init; }

        public required string Email { get; init; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ConfirmationToken)
                .NotEmpty()
                .WithResourceError(() => Resource.InvalidToken);
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithResourceError(() => Resource.InvalidEmail);
        }
    }


    public class Endpoint : Endpoint<Request, Results<NotFound, NoContent, BadRequestWhateverError>>
    {
        private readonly UserManager<User> userManager;

        public override void Configure()
        {
            AllowAnonymous();
            Post("confirm");
            Group<UsersGroup>();
        }

        public Endpoint(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public override async Task<Results<NotFound, NoContent, BadRequestWhateverError>> ExecuteAsync(Request req, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(req.Email);

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(req.ConfirmationToken));

            var identityResult = await userManager.ConfirmEmailAsync(user, decoded);

            if (!identityResult.Succeeded)
            {
                return new BadRequestWhateverError(identityResult.Errors);
            }

            return TypedResults.NoContent();
        }

    }

}
