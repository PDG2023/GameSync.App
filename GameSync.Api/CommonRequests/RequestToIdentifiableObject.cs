using FluentValidation;
using GameSync.Api.Extensions;

namespace GameSync.Api.CommonRequests;

public class RequestToIdentifiableObject
{
    public required int Id { get; init; }
}
public class RequestToIdentifiableObjectValidator : Validator<RequestToIdentifiableObject>
{
    public RequestToIdentifiableObjectValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithObjectDoesNotExistError();
    }
}