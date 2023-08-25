using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameSync.Api.Endpoints.Users.Me.Parties;

public class AddGame
{
    public class Endpoint : Endpoint<RequestToIdentifiableObject, Results<NotFound, Ok>>
    {
        private readonly GameSyncContext context;

        public Endpoint(GameSyncContext context) 
        {
            this.context = context;
        }

        public override Task<Results<NotFound, Ok>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);


            return base.ExecuteAsync(req, ct);
        }

    }
}
