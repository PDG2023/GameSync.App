using FluentValidation;
using FluentValidation.Results;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Common;

public class CredentialsValidator : Validator<IRequestWithCredentials>
{
    public CredentialsValidator()
    {

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode(nameof(Resources.Resource.InvalidEmail))
            .WithMessage(Resources.Resource.InvalidEmail);

        RuleFor(x => x.Password).CustomAsync(async (password, validatorContext, ct) =>
        {

            using var scope = CreateScope();
            var identityValidator = scope.Resolve<IPasswordValidator<User>>();
            var manager = scope.Resolve<UserManager<User>>();

            var failures = await identityValidator.ValidateAsync(manager, null!, password);

            if (!failures.Succeeded)
            {
                foreach (var failure in failures.Errors)
                {
                    validatorContext.AddFailure(new ValidationFailure
                    {
                        ErrorCode = failure.Code,
                        ErrorMessage = failure.Description
                    });
                }
            }
        });
    }
}
