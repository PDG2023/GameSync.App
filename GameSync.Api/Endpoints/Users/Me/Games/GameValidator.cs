using FluentValidation;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public class GameValidator : AbstractValidator<IGame>
{

    public GameValidator() 
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.MinPlayer).GreaterThan(0);
        RuleFor(x => x.MaxPlayer).GreaterThan(0).Must((request, maxPlayer) => maxPlayer >= request.MinPlayer);

        RuleFor(x => x.MinAge).GreaterThan(0).LessThan(120);


        RuleFor(x => x.DurationMinutes).GreaterThan(0);
        RuleFor(x => x.Description).MaximumLength(500).Must(description => description is null || !string.IsNullOrWhiteSpace(description));
    }
}

