using GameSync.Api.Persistence;
using GameSync.Business.Features.Search;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace GameSync.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameStoreSearcher searcher;
        private readonly GameSyncContext context;

        public GameController(GameStoreSearcher searcher, GameSyncContext context)
        {
            this.searcher = searcher;
            this.context = context;
        }

        [HttpGet("search")]
        public IEnumerable<Game> Get([FromQuery] string? term)
        {
            if (term is null)
                return Enumerable.Empty<Game>();
            return searcher.SearchGames(term);
        }

        [HttpPost("{name}")]
        public Game AddGame(string name)
        {
            var tracking = context.Games.Add(new Persistence.Entities.Game { Name = name });
            context.SaveChanges();
            return new Game(tracking.Entity.Name);
        }
    }
}