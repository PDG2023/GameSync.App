using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameSync.Api.Endpoints.Games;

public class GetGameRequest
{
    public required int Id { get; set; }
}

public class GetGameEndpoint : Endpoint<GetGameRequest, Results<Ok<Game>, NotFound>>
{
    public override void Configure()
    {

        Group<GamesGroup>();
    }
}
