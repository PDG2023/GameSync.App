using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities.Games;
using GameSync.Api.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public static class UpdateGame
{

    public class Request : RequestToIdentifiableObject, IGameRequest
    {
        public string? Name { get; init; }
        public int? MinPlayer { get; init; }
        public int? MaxPlayer { get; init; }
        public int? MinAge { get; init; }
        public string? Description { get; init; }
        public int? DurationMinute { get; init; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            Include(new GameRequestValidator());
            Include(new RequestToIdentifiableObjectValidator());
        }
    }

    public class Endpoint : Endpoint<Request, Results<NotFound, Ok<CustomGame>, BadRequestWhateverError>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Patch("{Id}");
            Group<CollectionGroup>();
        }

        public override async Task<Results<NotFound, Ok<CustomGame>, BadRequestWhateverError>> ExecuteAsync(Request req, CancellationToken ct)
        {

            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var game = await _context.CustomGames.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == req.Id);
            if (game is null)
            {
                return TypedResults.NotFound();
            }

            UpdateProperties(game, req);
            ThrowIfAnyErrors();

            _context.CustomGames.Update(game);
            await _context.SaveChangesAsync();

            return TypedResults.Ok(game);

        }

        private void UpdateProperties(CustomGame game, Request req)
        {

            if (req.MinPlayer is not null)
            {
                if (req.MinPlayer > game.MaxPlayer)
                {
                    AddError(x => x.MinPlayer, Resource.MaxPlayerLowerThanMinPlayer, nameof(Resource.MaxPlayerLowerThanMinPlayer));
                }
                else
                {
                    game.MinPlayer = req.MinPlayer.Value;
                }
            }

            if (req.MaxPlayer is not null)
            {
                if (req.MaxPlayer < game.MinPlayer)
                {
                    AddError(x => x.MaxPlayer, Resource.MaxPlayerLowerThanMinPlayer, nameof(Resource.MaxPlayerLowerThanMinPlayer));
                }
                else
                {
                    game.MaxPlayer = req.MaxPlayer.Value;
                }
            }


            if (req.Name is not null)
            {
                game.Name = HttpUtility.HtmlEncode(req.Name);
            }

            if (req.Description is not null)
            {
                game.Description = HttpUtility.HtmlEncode(req.Description);
            }

            if (req.DurationMinute is not null)
            {
                game.DurationMinute = req.DurationMinute;
            }

            if (req.MinAge is not null)
            {
                game.MinAge = req.MinAge.Value;
            }

        }

    }
}
