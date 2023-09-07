using FluentValidation;
using GameSync.Api.CommonRequests;
using GameSync.Api.Extensions;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty;

public static class UpdateParty
{

    public class Request : RequestToIdentifiableObject
    {
        public string? Name { get; init; }
        public string? Location { get; init; }
        public DateTime? DateTime { get; init; }
    }


    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .Must(x => x is null || !string.IsNullOrWhiteSpace(x))
                .WithObjectDoesNotExistError();

            RuleFor(x => x.DateTime)
                .GreaterThan(DateTime.Now)
                .When(x => x is not null)
                .WithResourceError(() => Resource.DateTimeMustBeAfterNow);

            Include(new RequestToIdentifiableObjectValidator());
        }
    }

    public class Endpoint : Endpoint<Request, Results<NotFound, Ok<Party>>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Patch("{Id}");
            Group<PartiesGroup>();
        }

        public override async Task<Results<NotFound, Ok<Party>>> ExecuteAsync(Request req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var party = await _context.Parties
                .Select(x => new Party
                {
                    DateTime = x.DateTime,
                    Location = x.Location,
                    Name = x.Name,
                    UserId = x.UserId,
                    InvitationToken = x.InvitationToken,
                    Id = x.Id
                })
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == req.Id);

            if (party is null)
            {
                return TypedResults.NotFound();
            }

            UpdateProperties(party, req);

            _context.Parties.Update(party);
            await _context.SaveChangesAsync();

            return TypedResults.Ok(party);
        }

        private void UpdateProperties(Party party, Request req)
        {
            if (req.Location is not null)
            {
                party.Location = HttpUtility.HtmlEncode(req.Location);
            }

            if (req.Name is not null)
            {
                party.Name = HttpUtility.HtmlEncode(req.Name);
            }

            if (req.DateTime is not null)
            {
                party.DateTime = req.DateTime.Value;
            }

            party.InvitationToken = party.InvitationToken;

        }

    }

}
