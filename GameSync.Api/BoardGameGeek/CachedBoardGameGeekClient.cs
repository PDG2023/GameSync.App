using GameSync.Api.BoardGameGeek.Schemas.Search;
using Microsoft.Extensions.Caching.Memory;

namespace GameSync.Api.BoardGameGeek;

public class CachedBoardGameGeekClient : BoardGameGeekClient
{
    private readonly IMemoryCache _memoryCache;

    public CachedBoardGameGeekClient(IMemoryCache cache)
    {
        _memoryCache = cache;
    }

    protected override async Task<IEnumerable<ThingItem>> GetDetailedThingsAsync(IEnumerable<string> ids)
    {
        var numberGames = ids.Count();
        var gamesToFetch = new List<string>(numberGames);
        var fetchedGames = new List<ThingItem>(numberGames);
        foreach (var id in ids)
        {
            if (_memoryCache.TryGetValue<ThingItem>(id, out var game))
            {
                fetchedGames.Add(game!);
            }
            else
            {
                gamesToFetch.Add(id);
            }
        }
        if (gamesToFetch.Count > 0)
        {
            var newGames = await base.GetDetailedThingsAsync(gamesToFetch);

            foreach (var newGame in newGames)
            {
                using var entry = _memoryCache.CreateEntry(newGame.Id.ToString());
                entry.Value = newGame;
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            }

            fetchedGames.AddRange(newGames);
        }

        return fetchedGames;

    }
}
