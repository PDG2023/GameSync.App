namespace GameSync.Api.Endpoints.Users.Me.Collection
{
    public class CollectionGroup : SubGroup<MeGroup>
    {
        public CollectionGroup()
        {
            Configure("collection", x => {
                x.DontAutoTag();
                x.Options(y => y.WithTags("Collection"));
            });
        }
    }
}
