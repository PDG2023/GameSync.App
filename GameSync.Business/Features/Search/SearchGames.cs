using GameSync.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Business.Features.Search;

public class GameStoreSearcher
{
    private readonly GameSyncContext _context;
    public GameStoreSearcher(GameSyncContext context) 
    {
        _context = context;
    }

    public IEnumerable<Game> SearchGames(string term)
    {
        return _context.Games
            .Where(x => x.Name.Contains(term))
            .Select(x => new Game(x.Name))
            .ToList();
    }
}


public record Game(string Name);