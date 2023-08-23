using FluentValidation;
using GameSync.Api.Common;
using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GameSync.Api.Endpoints.Users.Me.Parties;

public class UpdatePartyRequest : RequestToIdentifiableObject
{
    public string? Name { get; init; }
    public string? Location { get; init; }
    public DateTime? DateTime { get; init; }
}


public class UpdatePartyRequestValidator : Validator<UpdatePartyRequest>
{
    public UpdatePartyRequestValidator()
    {
        RuleFor(x => x.Name).Must(x => x is null || !string.IsNullOrWhiteSpace(x));
        RuleFor(x => x.DateTime).GreaterThan(DateTime.Now).When(x => x is not null);
        Include(new RequestToIdentifiableObjectValidator());
    }
}

public class UpdatePartyEndpoint : Endpoint<UpdatePartyRequest, Results<NotFound, Ok<Party>>>
{
    private readonly GameSyncContext _context;

    public UpdatePartyEndpoint(GameSyncContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Patch("{Id}");
        Group<PartiesGroup>();
    }

    public override async Task<Results<NotFound, Ok<Party>>> ExecuteAsync(UpdatePartyRequest req, CancellationToken ct)
    {
        var userId = User.ClaimValue(ClaimsTypes.UserId);

        var party = await _context.Parties
            .Select(x => new Party 
            { 
                DateTime = x.DateTime, 
                Location = x.Location, 
                Name = x.Name,
                UserId = x.UserId,
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

    private void UpdateProperties(Party party, UpdatePartyRequest req)
    {
        if (req.Location is not null)
        {
            party.Location = WebUtility.HtmlEncode(req.Location);
        }

        if (req.Name is not null)
        {
            party.Name =  WebUtility.HtmlEncode(req.Name);
        }

        if (req.DateTime is not null)
        {
            party.DateTime = req.DateTime.Value;
        }
    }

}
