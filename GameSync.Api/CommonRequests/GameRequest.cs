using FluentValidation;
using GameSync.Api.Extensions;
using GameSync.Api.Resources;

namespace GameSync.Api.CommonRequests;

public interface IGameRequest
{
    public string? Name { get; }
    public int? MinPlayer { get; }
    public int? MaxPlayer { get; }
    public int? MinAge { get; }
    public string? Description { get; }
    public int? DurationMinute { get; }
}


public class GameRequestValidator : AbstractValidator<IGameRequest>
{

    public GameRequestValidator()
    {
        RuleFor(x => x.Name)
            .Matches(@"^\S(.*\S)?$") // no whitespace at the beggining
            .When(x => x is not null)
            .WithResourceError(() => Resource.InvalidName);

        RuleFor(x => x.MinPlayer)
            .GreaterThan(0)
            .WithResourceError(() => Resource.NumberIsNegative);

        RuleFor(x => x.MaxPlayer)
            .GreaterThan(0)
            .When(x => x.MaxPlayer is not null)
            .WithResourceError(() => Resource.InvalidMinPlayer);

        RuleFor(x => x.MaxPlayer)
            .Must((req, maxPlayer) => maxPlayer >= req.MinPlayer)
            .WithResourceError(() => Resource.MaxPlayerLowerThanMinPlayer)
            .When(x => x.MaxPlayer is not null && x.MinPlayer is not null);

        RuleFor(x => x.MinAge)
            .GreaterThan(0)
            .LessThan(120)
            .WithResourceError(() => Resource.InvalidMinAge);

        RuleFor(x => x.DurationMinute)
            .GreaterThan(0)
            .WithResourceError(() => Resource.InvalidDuration);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null)
            .WithResourceError(() => Resource.DescriptionTooBig);
    }
}

