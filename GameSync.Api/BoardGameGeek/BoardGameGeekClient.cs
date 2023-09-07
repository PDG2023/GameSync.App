using GameSync.Api.BoardGameGeek.Schemas;
using GameSync.Api.BoardGameGeek.Schemas.Search;
using GameSync.Api.CommonResponses;
using GameSync.Api.Persistence.Entities.Games;
using System.Xml.Serialization;

namespace GameSync.Api.BoardGameGeek;
// https://boardgamegeek.com/wiki/page/BGG_XML_API2
public class BoardGameGeekClient
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

    private static TOutput Deserialize<TOutput>(Stream stream)
    {
        return (TOutput)new XmlSerializer(typeof(TOutput)).Deserialize(stream)!;
    }


    public async Task<IEnumerable<GamePreview>> SearchBoardGamesAsync(string term)
    {
        var body = await Client.GetStreamAsync($"search?type={Both}&query={term}");

        var searchResults = Deserialize<SearchResults>(body)!.Items;
        var itemsId = searchResults.Select(item => item.Id);

        var boardGames = await GetDetailedThingsAsync(itemsId);

        return searchResults
            .Join(boardGames, searchRes => int.Parse(searchRes.Id), bggGame => bggGame.Id, (searchResult, boardGame) => new  GamePreview
            {
                Id = int.Parse(searchResult.Id),
                Name = searchResult.Name.Value,
                IsExpansion = searchResult.Type == ExpansionType,
                YearPublished = int.Parse(searchResult.YearPublished?.Value ?? "0"),
                ImageUrl = boardGame.Image,
                ThumbnailUrl = boardGame.Thumbnail,
            });
    }

    protected virtual async Task<IEnumerable<ThingItem>> GetDetailedThingsAsync(IEnumerable<string> ids)
    {
        var idsQueryParam = string.Join(',', ids);
        var things = await Client.GetStreamAsync($"thing?id={idsQueryParam}");
        var results = Deserialize<ThingRoot>(things);
        return results.Items;

    }

    public async Task<IEnumerable<IGame>> GetBoardGamesDetailAsync(IEnumerable<int> ids)
    {
        var detail = await GetDetailedThingsAsync(ids.Select(x => x.ToString()));
        return detail.Where(x => x is not null).Select(thing => new GameDetail
        {
            Id = thing.Id,
            Description = thing.Description,
            MaxPlayer = thing.MaxPlayers?.ValueAsInt ?? 0,
            MinPlayer = thing.MinPlayers?.ValueAsInt ?? 0,
            DurationMinute = thing.PlayingTime?.ValueAsInt,
            MinAge = thing.MinAge?.ValueAsInt ?? 0,
            ImageUrl = thing.Image,
            ThumbnailUrl = thing.Thumbnail,
            YearPublished = thing.YearPublished?.ValueAsInt ?? 0,
            IsExpansion = (thing.Type ?? BoardGameType) == ExpansionType,
            Name = thing.Names?.FirstOrDefault(x => x.Type == "primary")?.Value ?? thing.Names?.FirstOrDefault()?.Value ?? string.Empty
        });
    }

}
