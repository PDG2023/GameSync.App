using Microsoft.AspNetCore.SignalR;

namespace GameSync.Api.Endpoints.LiveVote;

public class VoteHub : Hub
{
    public async Task RegisterForVoteFeed(int partyId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, partyId.ToString());
    }
}
