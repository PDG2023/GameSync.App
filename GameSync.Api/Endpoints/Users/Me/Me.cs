using Microsoft.AspNetCore.Http.HttpResults;

namespace GameSync.Api.Endpoints.Users.Me;

public static class Me
{
    public class Endpoint : EndpointWithoutRequest<NoContent>
    {

        public override void Configure()
        {
            Get(string.Empty);
            Group<MeGroup>();
        }

        public override Task<NoContent> ExecuteAsync(CancellationToken ct)
        {
            return Task.FromResult(TypedResults.NoContent());
        }

    }
}