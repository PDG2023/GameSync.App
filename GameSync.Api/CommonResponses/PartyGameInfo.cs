using GameSync.Api.Persistence.Entities;

namespace GameSync.Api.CommonResponses;

public class PartyGameInfo
{

    public PartyGameInfo() { }

    public PartyGameInfo(PartyGame pg) 
    {
        Id = pg.Id;
        GameImageUrl = pg is PartyBoardGameGeekGame ? ((PartyBoardGameGeekGame)pg).BoardGameGeekGame.ImageUrl : ((PartyCustomGame)pg).Game.ImageUrl;
        GameName = pg is PartyBoardGameGeekGame ? ((PartyBoardGameGeekGame)pg).BoardGameGeekGame.Name : ((PartyCustomGame)pg).Game.Name;
        WhoVotedNo = pg.Votes == null ? null : pg.Votes.Where(g => g.VoteYes == false).Select(v => v.UserId == null ? v.UserName : v.User.UserName).ToArray();
        WhoVotedYes = pg.Votes == null ? null : pg.Votes.Where(g => g.VoteYes == true).Select(v => v.UserId == null ? v.UserName : v.User.UserName).ToArray();
    }


    public int Id { get; set; }
    public string? GameImageUrl { get; init; }
    public string? GameName { get; init; }
    public IEnumerable<string>? WhoVotedYes { get; init; }
    public int CountVotedYes => WhoVotedYes?.Count() ?? 0;
    public IEnumerable<string>? WhoVotedNo { get; init; }
    public int CountVotedNo => WhoVotedNo?.Count() ?? 0;

}
