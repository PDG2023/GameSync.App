using FluentValidation;

namespace GameSync.Api.CommonRequests;

public class SingleMailRequestValidator : Validator<SingleMailRequest>
{
    public SingleMailRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}