
using Duende.IdentityServer.Models;
using FluentValidation;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Endpoints.Users.PasswordReset;

public class ChangePasswordRequest
{

    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string PasswordRepetition { get; init; }
    public required string Token { get; init; }

}

public class ChangePasswordRequestValidator : Validator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.PasswordRepetition)
            .NotEmpty()
            .Must((req, x) => req.Password == x)
            .WithMessage(Resources.Resource.PasswordDontMatch);
    }
}

public class ChangePasswordEndpoint : Endpoint<ChangePasswordRequest, Results<Ok, NotFound, BadRequestWhateverError>>
{
    private readonly UserManager<User> _manager;

    public ChangePasswordEndpoint(UserManager<User> manager)
    {
        _manager = manager;
    }

    public override void Configure()
    {
        Post("change-password");
        Group<PasswordResetGroup>();
    }

    public override async Task<Results<Ok, NotFound, BadRequestWhateverError>> ExecuteAsync(ChangePasswordRequest req, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            return await Task.FromResult(new BadRequestWhateverError(ValidationFailures));
        }

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
