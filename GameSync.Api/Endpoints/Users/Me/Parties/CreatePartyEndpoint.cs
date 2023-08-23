using FluentValidation;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Namotion.Reflection;
using System.Text.Encodings.Web;
using System.Web;

namespace GameSync.Api.Endpoints.Users.Me.Parties;
public class CreatePartyRequest
{
    public required string Name { get; init; }
    public string? Location { get; init; }
    public required DateTime DateTime { get; init; }

}

public class CreatePartyRequestValidator : Validator<CreatePartyRequest>
{
    public CreatePartyRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.DateTime)
            .GreaterThan(DateTime.Now)
            .WithMessage(Resources.Resource.DateTimeMustBeAfterNow)
            .WithErrorCode(nameof(Resources.Resource.DateTimeMustBeAfterNow));
    }
}

public class CreatePartyEndpoint : Endpoint<CreatePartyRequest, PartyPreview>
{
    private readonly GameSyncContext _context;

    public CreatePartyEndpoint(GameSyncContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Post(string.Empty);
        Group<PartiesGroup>();
    }

    public override async Task<PartyPreview> ExecuteAsync(CreatePartyRequest req, CancellationToken ct)
    {
        var userId = User.ClaimValue(ClaimsTypes.UserId);
        var party = new Party
        {
            DateTime = req.DateTime,
            Location = req.Location,
            Name = req.Name,
            UserId = userId!,
            Games = null
        };

        var partyTracking = await _context.Parties.AddAsync(party);
        await _context.SaveChangesAsync();

        return new PartyPreview 
        { 
            Id = party.Id, 
            Name = HttpUtility.HtmlEncode(req.Name), 
            NumberOfGames = 0, 
            DateTime = req.DateTime, 
            Location = HttpUtility.HtmlEncode(req.Location)
        };
    }

}
