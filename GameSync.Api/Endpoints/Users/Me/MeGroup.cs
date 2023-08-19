namespace GameSync.Api.Endpoints.Users.Me;

public class MeGroup : SubGroup<UsersGroup>
{
    public MeGroup()
    {
        Configure("me", definition =>
        {
            definition.Claims(ClaimsNames.UserId);
        });
    }
}
