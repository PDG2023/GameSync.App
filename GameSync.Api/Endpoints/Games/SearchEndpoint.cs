using GameSync.Business.Features.Search;
using Microsoft.AspNetCore.Authorization;

namespace GameSync.Api.Endpoints.GameSearch;

public class SearchGameRequest
{
    [QueryParam]
    public required string Query { get; set; }
}


[HttpGet("games/search")]
[AllowAnonymous]
public class SearchEndpoint : Endpoint<SearchGameRequest, IEnumerable<BoardGameSearchResult>>
{
    private readonly IGameSearcher gameSearcher;

    public SearchEndpoint(IGameSearcher gameSearcher) 
    {
        this.gameSearcher = gameSearcher;
    }

    public override async Task<IEnumerable<BoardGameSearchResult>> ExecuteAsync(SearchGameRequest req, CancellationToken ct)
    {
        return await gameSearcher.SearchBoardGamesAsync(req.Query);
    }
}
