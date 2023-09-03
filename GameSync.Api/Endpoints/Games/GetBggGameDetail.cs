using GameSync.Api.BoardGameGeek;
using GameSync.Api.CommonRequests;
using GameSync.Api.CommonResponses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameSync.Api.Endpoints.Games;

public static class GetBggGameDetail
{

    public class Endpoint : Endpoint<RequestToIdentifiableObject, Results<Ok<GameDetail>, NotFound, BadRequestWhateverError>>
    {
        private readonly BoardGameGeekClient _client;

        public Endpoint(BoardGameGeekClient client)
        {
            _client = client;
        }

        public override void Configure()
        {
            Get("{Id}");
            Group<GamesGroup>();
        }

        public override async Task<Results<Ok<GameDetail>, NotFound, BadRequestWhateverError>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
        {

            var fetchedGames = await _client.GetBoardGamesDetailAsync(new[] { req.Id });
            var game = fetchedGames.FirstOrDefault();
            if (game is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(game);
        }

    }

}
