using GameSync.Api.Common;
using GameSync.Business.BoardGamesGeek;
using Microsoft.AspNetCore.Http.HttpResults;
using BoardGameGeekGame = GameSync.Business.BoardGameGeek.Model.BoardGameGeekGame;

namespace GameSync.Api.Endpoints.Games;

public class GetGameEndpoint : Endpoint<SingleGameRequest, Results<Ok<BoardGameGeekGame>, NotFound, BadRequestWhateverError>>
{
    private readonly BoardGameGeekClient _client;

    public GetGameEndpoint(BoardGameGeekClient client)
    {
        _client = client;
    }

    public override void Configure()
    {
        Get("{Id}");
        Group<GamesGroup>();
    }

    public override async Task<Results<Ok<BoardGameGeekGame>, NotFound, BadRequestWhateverError>> ExecuteAsync(SingleGameRequest req, CancellationToken ct)
    {
        var fetchedGames = await _client.GetBoardGamesDetailAsync(new[] {req.Id});
        var game = fetchedGames.FirstOrDefault();
        if (game is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(game);
    }
}
