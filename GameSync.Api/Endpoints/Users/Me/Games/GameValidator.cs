using FluentValidation;
using GameSync.Api.Resources;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public class GameValidator : AbstractValidator<GameRequest>
{

    public GameValidator() 
    {
        RuleFor(x => x.Name)
            .Matches(@"^\S(.*\S)?$") // no whitespace at the beggining
            .When(x => x is not null);

        RuleFor(x => x.MinPlayer)
            .GreaterThan(0);

        RuleFor(x => x.MaxPlayer)
            .GreaterThan(0)
            .When(x => x.MaxPlayer is not null);

        RuleFor(x => x.MaxPlayer)
            .Must((req, maxPlayer) => maxPlayer >= req.MinPlayer)
            .WithErrorCode(nameof(Resource.MaxPlayerLowerThanMinPlayer))
            .WithMessage(Resource.MaxPlayerLowerThanMinPlayer)
            .When(x => x.MaxPlayer is not null && x.MinPlayer is not null);

        RuleFor(x => x.MinAge)
            .GreaterThan(0)
            .LessThan(120);

        RuleFor(x => x.DurationMinute)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null);
    }
}

