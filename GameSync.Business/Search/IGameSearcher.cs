namespace GameSync.Business.Search;

public interface IGameSearcher
{
    public Task<IEnumerable<BoardGameSearchResult>> SearchBoardGamesAsync(string term);
}
