using FluentValidation;
using GameSync.Business.BoardGameGeek.Models;
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


[HttpGet(ENDPOINT_ROUTE)]
[AllowAnonymous]
public class SearchEndpoint : Endpoint<SearchGameRequest, PaginatedResult<BoardGameSearchResult>>
{
    public const string ENDPOINT_ROUTE = "games/search";

    private readonly BoardGameGeekClient _client;

    public SearchEndpoint(BoardGameGeekClient client)
    {
        _client = client;
    }

    public override async Task<PaginatedResult<BoardGameSearchResult>> ExecuteAsync(SearchGameRequest req, CancellationToken ct)
    {
        var collection = await _client.SearchBoardGamesAsync(req.Query);
        var request = HttpContext.Request;
        return new PaginatedResult<BoardGameSearchResult>(
            collection,
            req.PageSize, 
            req.Page, 
            $"{request.Scheme}://{request.Host}/api/{ENDPOINT_ROUTE}?{nameof(req.Query)}={HttpUtility.UrlEncode(req.Query)}");
    }
}
