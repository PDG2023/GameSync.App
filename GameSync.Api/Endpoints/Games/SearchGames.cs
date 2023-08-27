using FluentValidation;
using GameSync.Business.BoardGameGeek.Model;
using GameSync.Business.BoardGamesGeek;
using System.Web;

namespace GameSync.Api.Endpoints.Games;

public static class SearchGames
{

    public class Request
    {
        [QueryParam]
        public required string Query { get; set; }

        [QueryParam]
        public int PageSize { get; set; }

        [QueryParam]
        public int Page { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Query).NotEmpty();
            RuleFor(r => r.PageSize).ExclusiveBetween(0, 101);
            RuleFor(r => r.Page).GreaterThanOrEqualTo(0);
        }
    }


    public class Endpoint : Endpoint<Request, PaginatedResult<BoardGameSearchResult>>
    {
        public const string Route = "search";

        private readonly BoardGameGeekClient _client;

        public Endpoint(BoardGameGeekClient client)
        {
            _client = client;
        }

        public override void Configure()
        {
            Get(Route);
            Group<GamesGroup>();
        }

        public override async Task<PaginatedResult<BoardGameSearchResult>> ExecuteAsync(Request req, CancellationToken ct)
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

}
