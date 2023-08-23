using FluentValidation;

namespace GameSync.Api.CommonRequests;

public class RequestToIdentifiableObjectValidator : Validator<RequestToIdentifiableObject>
{
    public RequestToIdentifiableObjectValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}