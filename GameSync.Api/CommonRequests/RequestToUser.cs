using FluentValidation;
using GameSync.Api.Extensions;
using GameSync.Api.Resources;

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
            .WithResourceError(() => Resource.InvalidEmail);

    }
}