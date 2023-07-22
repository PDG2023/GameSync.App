using GameSync.Business.Features.Search;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameSync.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<Game> Get([FromQuery][Required] string term)
        {
            var store = new[]
            {
                new Game("my game"),
                new Game("second game")
            };

            var searcher = new GameStoreSearcher(store);

            return searcher.SearchGames(term);
        }
    }
}