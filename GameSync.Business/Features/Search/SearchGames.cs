namespace GameSync.Business.Features.Search;

public class GameStoreSearcher
{
    private readonly IReadOnlyCollection<Game> _store;
    public GameStoreSearcher(IReadOnlyCollection<Game> store) 
    {
        _store = store;
    }

    public IEnumerable<Game> SearchGames(string term)
    {
        return _store
            .Where(game => game.Name.Contains(term));
    }
}


public record Game(string Name);