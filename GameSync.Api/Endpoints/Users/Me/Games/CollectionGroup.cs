namespace GameSync.Api.Endpoints.Users.Me.Collection
{
    public class CollectionGroup : SubGroup<MeGroup>
    {
        public CollectionGroup()
        {
            Configure("games", x => {
                x.DontAutoTag();
                x.Options(y => y.WithTags("User's games"));
            });
        }
    }
}
