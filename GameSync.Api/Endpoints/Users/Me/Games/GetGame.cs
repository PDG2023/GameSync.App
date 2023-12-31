﻿using GameSync.Api.CommonRequests;
using GameSync.Api.CommonResponses;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public static class GetGame
{

    public class Endpoint : Endpoint<RequestToIdentifiableObject, Results<Ok<GameDetail>, NotFound>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("{Id}");
            Group<CollectionGroup>();
        }

        public override async Task<Results<Ok<GameDetail>, NotFound>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
        {

            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var game = await _context.CustomGames
                .FirstOrDefaultAsync(g => g.UserId == userId && g.Id == req.Id);

            if (game is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(new GameDetail(game));
        }
    }

}
