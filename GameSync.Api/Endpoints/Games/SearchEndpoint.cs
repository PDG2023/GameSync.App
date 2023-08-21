using FluentValidation;
using GameSync.Api.Endpoints.Games;
using GameSync.Business.BoardGameGeek.Model;
using GameSync.Business.BoardGamesGeek;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using System.Web;

namespace GameSync.Api.Endpoints.GameSearch;

public class SearchGameRequest
{
    [QueryParam]
    public required string Query { get; set; }

    [QueryParam]
    public int PageSize { get; set; }

    [QueryParam]
    public int Page { get; set; }
}

public class PaginationValidator : Validator<SearchGameRequest>
{
    public PaginationValidator()
    {
        RuleFor(r => r.PageSize).ExclusiveBetween(0, 101);
        RuleFor(r => r.Page).GreaterThanOrEqualTo(0);
    }
}


public class SearchEndpoint : Endpoint<SearchGameRequest, PaginatedResult<BoardGameSearchResult>>
{
    public const string Route = "search";

    private readonly BoardGameGeekClient _client;

    public SearchEndpoint(BoardGameGeekClient client)
    {
        _client = client;
    }

    public override void Configure()
    {
        Get(Route);
        Group<GamesGroup>();
    }

    public override async Task<PaginatedResult<BoardGameSearchResult>> ExecuteAsync(SearchGameRequest req, CancellationToken ct)
    {
        var collection = await _client.SearchBoardGamesAsync(req.Query);
        var request = HttpContext.Request;
        return new PaginatedResult<BoardGameSearchResult>(
            collection,
            req.PageSize, 
            req.Page, 
            $"{request.Scheme}://{request.Host}/api/{GamesGroup.Prefix}/{Route}?{nameof(req.Query)}={HttpUtility.UrlEncode(req.Query)}");
    }
}
