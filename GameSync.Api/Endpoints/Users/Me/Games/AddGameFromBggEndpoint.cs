using Duende.IdentityServer.Events;
using FluentValidation;
using GameSync.Business.BoardGamesGeek;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public class AddGameFromBggRequest
{
    [JsonPropertyName("ids")]
    public required IEnumerable<int> IDs { get; init; } 
}

public class AddGameFromBggValidator : Validator<AddGameFromBggRequest>
{
    public AddGameFromBggValidator()
    {
        RuleFor(x => x.IDs).NotEmpty();
        RuleForEach(x => x.IDs).GreaterThan(0);
    }
}

public class AddGameFromBggEndpoint : Endpoint<AddGameFromBggRequest, Results<NotFound<IEnumerable<int>>, Ok>>
{
    private readonly BoardGameGeekClient _client;

    public AddGameFromBggEndpoint(BoardGameGeekClient client)
    {
        _client = client;
    }

    public override void Configure()
    {
        Post("from-bgg");
        Group<CollectionGroup>();
    }

    public override async Task<Results<NotFound<IEnumerable<int>>, Ok>> ExecuteAsync(AddGameFromBggRequest req, CancellationToken ct)
    {
        var games = await _client.GetBoardGamesDetailAsync(req.IDs);
        var retrievedIds = games.Select(game => game.Id);
        if (games.Count() != req.IDs.Count())
        {
            var nonExistingIds = req.IDs.Except(retrievedIds);
            return TypedResults.NotFound(nonExistingIds);
        }



        return await base.ExecuteAsync(req, ct);
    }
}
