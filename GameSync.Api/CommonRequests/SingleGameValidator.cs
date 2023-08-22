using FluentValidation;

namespace GameSync.Api.Common;

public class SingleGameRequestValidator : Validator<SingleGameRequest>
{
    public SingleGameRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}