using GameSync.Business.Features.Search;
using Microsoft.AspNetCore.Mvc;

namespace GameSync.Api.Endpoints.GameSearch
{
    [ApiController]
    [Route("games/search")]
    public class Controller : ControllerBase
    {
        private readonly IGameSearcher gameSearcher;

        public Controller(IGameSearcher gameSearcher)
        {
            this.gameSearcher = gameSearcher;
        }

        [HttpGet]
        public async Task<IEnumerable<BoardGameSearchResult>> SearchGames([FromQuery] string query)
        {
            return await gameSearcher.SearchBoardGamesAsync(query);
        }
    }
}