using FluentValidation;

namespace GameSync.Api.Endpoints.Users.IndividualUser;

public class SingleMailRequestValidator : Validator<SingleMailRequest>
{
    public SingleMailRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}