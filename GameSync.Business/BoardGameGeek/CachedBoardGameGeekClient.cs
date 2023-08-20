using GameSync.Business.BoardGameGeek.Model;
using GameSync.Business.BoardGamesGeek;
using GameSync.Business.BoardGamesGeek.Schemas.Thing;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.InteropServices;

namespace GameSync.Business.BoardGameGeek;

public class CachedBoardGameGeekClient : BoardGameGeekClient
{

    private static Dictionary<string, ThingItem> testCache = new Dictionary<string, ThingItem>();
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
                Console.WriteLine("hit");
                fetchedGames.Add(game!);
            }
            else
            {
                gamesToFetch.Add(id);
            }
        }

        var newGames = await base.GetDetailedThingsAsync(gamesToFetch);

        foreach (var newGame in newGames)
        {
            using var entry = _memoryCache.CreateEntry(newGame.Id);
            entry.Value = newGame;
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            
        }

        fetchedGames.AddRange(newGames);

        return fetchedGames;

    }
}
