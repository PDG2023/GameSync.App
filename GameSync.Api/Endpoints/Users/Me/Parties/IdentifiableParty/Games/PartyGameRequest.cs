using FluentValidation;
using GameSync.Api.Persistence.Entities;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;

public class PartyGameRequest
{
    public required int GameId { get; init; }

    public required int PartyId { get; init; }
}

public class PartyGameRequestValidator : Validator<PartyGameRequest>
{
    public PartyGameRequestValidator()
    {
        RuleFor(x => x.GameId).GreaterThan(0);
        RuleFor(x => x.PartyId).GreaterThan(0);
    }
}
