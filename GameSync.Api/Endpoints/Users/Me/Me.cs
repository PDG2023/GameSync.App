using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me;

public static class Me
{

    public class Response
    {
        public required string UserName { get; init; }
        public required string Email { get; init; }
    }

    public class Endpoint : EndpointWithoutRequest<Response>
    {
        private readonly GameSyncContext _ctx;

        public Endpoint(GameSyncContext ctx)
        {
            _ctx = ctx;
        }

        public override void Configure()
        {
            Get(string.Empty);
            Group<MeGroup>();
        }

        public override async Task<Response> ExecuteAsync(CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var user = await _ctx.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new User
                {
                    UserName = u.UserName,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();  

            return new Response { Email = user.Email, UserName = user.UserName };
        }

    }
}