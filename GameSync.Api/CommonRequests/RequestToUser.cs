using FluentValidation;

namespace GameSync.Api.CommonRequests;

public class RequestToUser
{

    public required string Email { get; init; }
}


public class RequestToUserValidator : Validator<RequestToUser>
{
    public RequestToUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode(nameof(Resources.Resource.InvalidEmail))
            .WithMessage(Resources.Resource.InvalidEmail);

    }
}