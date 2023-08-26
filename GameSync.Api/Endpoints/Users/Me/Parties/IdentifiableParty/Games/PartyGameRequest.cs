namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;

public class PartyGameRequest
{
    public required int GameId { get; init; }
    public required int PartyId { get; init; }
}
