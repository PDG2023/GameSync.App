namespace GameSync.Api.Endpoints.Users.Me.Parties;

public class PartiesGroup : SubGroup<MeGroup>
{
    public PartiesGroup()
    {
        Configure("parties", _ => { });
    }
}
