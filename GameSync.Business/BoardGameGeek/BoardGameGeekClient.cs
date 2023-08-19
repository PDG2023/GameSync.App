using GameSync.Business.BoardGamesGeek.Schemas;
using GameSync.Business.BoardGamesGeek.Schemas.Search;
using GameSync.Business.BoardGamesGeek.Schemas.Thing;
using GameSync.Business.Search;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;

namespace GameSync.Business.BoardGamesGeek;
// https://boardgamegeek.com/wiki/page/BGG_XML_API2
public class BoardGameGeekClient : IGameSearcher
{

    public const string BoardGameType = "boardgame";
    public const string ExpansionType = "boardgameexpansion";
    public const string Both = $"{BoardGameType},{ExpansionType}";

    private static HttpClient? _instance = null;

    private static HttpClient Client
    {
        get
        {
            if (_instance is not null) return _instance;

            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10)
            };

            _instance = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://boardgamegeek.com/xmlapi2/")
            };

            return _instance;
        }
    }

    private TOutput Deserialize<TOutput>(Stream stream)
    {
        return (TOutput)new XmlSerializer(typeof(TOutput)).Deserialize(stream)!;
    }

    public async Task<IEnumerable<BoardGameSearchResult>> SearchBoardGamesAsync(string term)
    {
        var body = await Client.GetStreamAsync($"search?type={Both}&query={term}");

        var searchResults = Deserialize<SearchResults>(body)!.Items;
        var itemsId = searchResults.Select(item => item.Id).ToList();

        var boardGames = await GetDetailedThingsAsync(itemsId);

        return boardGames.Zip(searchResults).Select(pair =>
        {
            var (boardGame, searchResult) = pair;
            return new BoardGameSearchResult
            {
                Id = int.Parse(searchResult.Id),
                Name = searchResult.Name.Value,
                IsExpansion = searchResult.Type == ExpansionType,
                YearPublished = int.Parse(searchResult.YearPublished.Value),
                ImageUrl = boardGame.Image,
                ThumbnailUrl = boardGame.Thumbnail,
            };
        });
    }

    private async Task<IEnumerable<ThingItem>> GetDetailedThingsAsync(IEnumerable<string> ids)
    {
        var idsQueryParam = string.Join(',', ids);
        var things = await Client.GetStreamAsync($"thing?id={idsQueryParam}");
        var results = Deserialize<ThingRoot>(things);
        return results.Items;

    }

}
