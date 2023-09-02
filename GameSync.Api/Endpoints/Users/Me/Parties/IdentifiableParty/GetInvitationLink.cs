using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Business.BoardGameGeek.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Web;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty;

public static class GetInvitationLink
{

    public class Endpoint : Endpoint<RequestToIdentifiableObject, Results<NotFound, Ok<string>>>
    {
        private readonly GameSyncContext _context;
        private readonly IConfiguration _configuration;

        public Endpoint(GameSyncContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public override void Configure()
        {
            Get("{Id}/invitation-token");
            Group<PartiesGroup>();
        }

        public override async Task<Results<NotFound, Ok<string>>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var party = await _context
                .Parties
                .Where(p => p.Id == req.Id && p.UserId == userId)
                .Select(p => new Party { InvitationToken = p.InvitationToken, Id = p.Id })
                .FirstOrDefaultAsync();
            if (party is null)
            {
                return TypedResults.NotFound();
            }

            if (party.InvitationToken is null)
            {
                _context.Parties.Attach(party);
                var guid = Guid.NewGuid();
                party.InvitationToken = Base64UrlEncoder.Encode(guid.ToByteArray());
                await _context.SaveChangesAsync();
            }

            var url = GetAbsoluteUrlToDestination(party.InvitationToken);

            return TypedResults.Ok(url);
        }

        private string GetAbsoluteUrlToDestination(string token)
        {
            var route = (_configuration["FrontPathToInvitedParty"] ?? string.Empty)
                .Replace("{InvitationToken}", token);
            var request = HttpContext.Request;
            return $"{request.Scheme}://{request.Host}/{route}";
        }

    }
}
