using FluentValidation;
using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games;


public class DeleteGameEndpoint : Endpoint<RequestToIdentifiableObject, Results<BadRequestWhateverError, NotFound, Ok>>
{
    private readonly GameSyncContext _context;

    public DeleteGameEndpoint(GameSyncContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete("{Id}");
        Group<CollectionGroup>();
    }

    public override async Task<Results<BadRequestWhateverError, NotFound, Ok>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
    {

        var userId = User.ClaimValue(ClaimsTypes.UserId);
        var gameExists = await _context.Games.AnyAsync(game => game.Id == req.Id && userId == game.UserId);
        
        if (!gameExists)
        {
            return TypedResults.NotFound();
        }

        await _context.Games.Where(game => game.Id == req.Id).ExecuteDeleteAsync();

        return TypedResults.Ok();

    }



}
