using FluentValidation;

namespace GameSync.Api.Endpoints.Users.User;

public class SingleMailRequest
{
    public required string Email { get; set; }
}

public class SingleMailRequestValidator : Validator<SingleMailRequest>
{
    public SingleMailRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}