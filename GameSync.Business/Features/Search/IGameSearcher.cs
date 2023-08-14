namespace GameSync.Business.Features.Search;

public interface IGameSearcher
{
    public Task<IEnumerable<BoardGameSearchResult>> SearchBoardGamesAsync(string term);
}
