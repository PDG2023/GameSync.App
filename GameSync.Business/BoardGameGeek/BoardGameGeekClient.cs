using GameSync.Business.BoardGamesGeek.Schemas;
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
        return (TOutput)new XmlSerializer(typeof(TOutput)).Deserialize(stream);
    }

    public async Task<IEnumerable<BoardGameSearchResult>> SearchBoardGamesAsync(string term)
    {
        var body = await Client.GetStreamAsync($"search?type={Both}&query={term}");

        var results = Deserialize<Results>(body)!;

        return results.Items.Select(searchResult =>
        {
            return new BoardGameSearchResult
            {
                Id = int.Parse(searchResult.Id),
                Name = searchResult.Name.Value,
                IsExpansion = searchResult.Type == ExpansionType,
                YearPublished = int.Parse(searchResult.YearPublished.Value)
            };
        });
    }

}
