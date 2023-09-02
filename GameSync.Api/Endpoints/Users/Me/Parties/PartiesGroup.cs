namespace GameSync.Api.Endpoints.Users.Me.Parties;

public class PartiesGroup : SubGroup<MeGroup>
{
    public PartiesGroup()
    {
        Configure("parties", x => {
            x.DontAutoTag();
            x.Options(x => x.WithTags("Parties"));
        });
    }
}
