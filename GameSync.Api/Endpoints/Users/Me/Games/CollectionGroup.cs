using FastEndpoints;

namespace GameSync.Api.Endpoints.Users.Me.Games
{
    public class CollectionGroup : SubGroup<MeGroup>
    {
        public CollectionGroup()
        {
            Configure("games", x => {
                x.DontThrowIfValidationFails();
                x.DontAutoTag();
                x.Options(y => y.WithTags("User's games"));
            });
        }
    }
}
