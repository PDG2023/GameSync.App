﻿using FluentValidation;
using GameSync.Api.CommonRequests;
using GameSync.Api.Extensions;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Persistence.Entities.Games;
using GameSync.Api.Resources;
using System.Net;
using System.Text.Json.Serialization;
using System.Web;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public static class CreateGame
{

    public class Request : IGameRequest
    {
        public required string Name { get; init; }
        public required int MinPlayer { get; init; }
        public required int MaxPlayer { get; init; }
        public required int MinAge { get; init; }
        public string? Description { get; init; }
        public int? DurationMinute { get; init; }

        [JsonIgnore]
        int? IGameRequest.MinPlayer => MinPlayer;

        [JsonIgnore]
        int? IGameRequest.MaxPlayer => MaxPlayer;

        [JsonIgnore]
        int? IGameRequest.MinAge => MinAge;
    }


    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .WithResourceError(() => Resource.InvalidName);

            Include(new GameRequestValidator());
        }
    }

    public class Endpoint : Endpoint<Request, CustomGame>
    {
        private readonly GameSyncContext _context;
        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Post(string.Empty);
            Group<CollectionGroup>();
        }

        public override async Task<CustomGame> ExecuteAsync(Request req, CancellationToken ct)
        {
            var trackingGame = await _context.CustomGames.AddAsync(RequestToGame(req), ct);
            await _context.SaveChangesAsync(ct);
            return trackingGame.Entity;
        }

        private CustomGame RequestToGame(Request r)
        {
            return new CustomGame
            {
                MaxPlayer = r.MaxPlayer,
                MinPlayer = r.MinPlayer,
                Name = HttpUtility.HtmlEncode(r.Name),
                MinAge = r.MinAge,
                UserId = User.ClaimValue(ClaimsTypes.UserId)!,
                Description = HttpUtility.HtmlEncode(r.Description),
                DurationMinute = r.DurationMinute
            };
        }
    }


}
