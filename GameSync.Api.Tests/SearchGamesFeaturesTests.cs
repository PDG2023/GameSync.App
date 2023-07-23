using GameSync.Api.Persistence;
using GameSync.Business.Features.Search;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Tests
{
    [TestClass]
    public class SearchGamesFeaturesTests
    {

        GameSyncContext _context;
        GameStoreSearcher _searcher;
        SqliteConnection _connection;
        [TestInitialize]
        public void Init()
        {
            var connectionString = "Data Source=SearchGamesFeatures;Mode=Memory;Cache=Shared";
            _connection = new SqliteConnection(connectionString);
            _connection.Open();

            var opt = new DbContextOptionsBuilder<GameSyncContext>()
                .UseSqlite(_connection)
                .Options;
            _context = new GameSyncContext(opt);
            _context.Database.Migrate();
            SetupData();
            _searcher = new GameStoreSearcher(_context);
        }

        [TestCleanup]
        public void Cleanup() 
        {
            _connection.Close();
            _connection.Dispose();
        }

        private void SetupData()
        {
            _context.Games.Add(new Persistence.Entities.Game { Name = "Loups-garoups pour une nuit" });
            _context.Games.Add(new Persistence.Entities.Game { Name = "Loups-garoups de Thiercelieu" });
            _context.Games.Add(new Persistence.Entities.Game { Name = "Twister" });
            _context.SaveChanges();
        }


        [TestMethod]
        public void SearchingExistingTerm()
        {
            var needle = _searcher.SearchGames("Loups");
            Assert.AreEqual(needle.Count(), 2);
        }
    }
}