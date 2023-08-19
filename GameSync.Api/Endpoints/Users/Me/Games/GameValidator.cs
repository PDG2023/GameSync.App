using FluentValidation;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public class GameValidator : AbstractValidator<IGame>
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
            .When(x => x.MaxPlayer is not null && x.MinPlayer is not null);

        RuleFor(x => x.MinAge)
            .GreaterThan(0)
            .LessThan(120);

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .NotEmpty()
            .When(x => x.Description is not null);
    }
}

