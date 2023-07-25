using GameSync.Api.Persistence;
using GameSync.Business.Features.Search;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameSync.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly GameStoreSearcher searcher;

        public SearchController(GameStoreSearcher searcher)
        {
            this.searcher = searcher;
        }

        [HttpGet]
        public IEnumerable<Game> Get([FromQuery] string term)
        {
            return searcher.SearchGames(term);
        }
    }
}