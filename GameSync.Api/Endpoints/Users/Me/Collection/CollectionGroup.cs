namespace GameSync.Api.Endpoints.Users.Me.Collection
{
    public class CollectionGroup : SubGroup<MeGroup>
    {
        public CollectionGroup()
        {

            Configure("collection", _ => { });
        }
    }
}
