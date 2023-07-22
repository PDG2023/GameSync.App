using GameSync.Business.Features.Search;

namespace GameSync.Api.Tests
{
    [TestClass]
    public class SearchGamesFeaturesTests
    {
        private GameStoreSearcher _searcher =  new(new[]
        {
            new Game("Loups-garous de Thiercelieux"),
            new Game("Loups-garous pour une nuit"),
            new Game("Twister")
        });


        [TestMethod]
        public void SearchingExistingTerm()
        {
            var needle = _searcher.SearchGames("Loups");

            Assert.AreEqual(needle.Count(), 2);
        }
    }
}