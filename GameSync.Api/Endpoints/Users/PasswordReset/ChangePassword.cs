using FluentValidation;
using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Endpoints.Users.PasswordReset;

public static class ChangePassword
{
    public class Request : IRequestWithCredentials
    {

        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string PasswordRepetition { get; init; }
        public required string Token { get; init; }

    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            Include(new CredentialsValidator());
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.PasswordRepetition)
                .NotEmpty()
                .Must((req, x) => req.Password == x)
                .WithMessage(Resources.Resource.PasswordDontMatch);
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

            var changePasswordResult = await _manager.ResetPasswordAsync(user, req.Token, req.Password);

            if (!changePasswordResult.Succeeded)
            {
                return new BadRequestWhateverError(changePasswordResult.Errors);
            }

            return TypedResults.Ok();
        }
    }

}
