namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;

public class PartyGameGroup : SubGroup<PartiesGroup>
{
    public PartyGameGroup()
    {
        Configure("{PartyId}/games/{GameId}", x =>
        {
            x.DontAutoTag();
            x.Options(builder => builder.WithTags("Party's games"));
        });
    }
}
