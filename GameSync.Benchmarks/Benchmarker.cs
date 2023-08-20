using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using GameSync.Business.BoardGameGeek;
using GameSync.Business.BoardGameGeek.Model;
using GameSync.Business.BoardGamesGeek;
using Microsoft.Extensions.Caching.Memory;


[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Monitoring, RuntimeMoniker.Net70)]
[IterationCount(5)]
public class BggClient
{

    private static BoardGameGeekClient _noCacheClient = new();
    private static CachedBoardGameGeekClient _cachedClient = new(new MemoryCache(new MemoryCacheOptions()));
    private static IEnumerable<int> ids = Enumerable.Range(1000, 1100).ToList();


    [Benchmark]
    public async Task<List<BoardGameGeekGame>> GetDetailsNoCache()
    {
        return (await _noCacheClient.GetBoardGamesDetailAsync(ids)).ToList();
    }

    [Benchmark]
    public async Task<List<BoardGameGeekGame>> GetDetailsCached()
    {
        return (await _cachedClient.GetBoardGamesDetailAsync(ids)).ToList();
    }
    

    [Benchmark]
    public async Task<List<BoardGameSearchResult>> SearchCluedoNoCache()
    {
        return (await _noCacheClient.SearchBoardGamesAsync("Cluedo")).ToList();
    }

    [Benchmark]
    public async Task<List<BoardGameSearchResult>> SearchCluedoCache()
    {
        return (await _cachedClient.SearchBoardGamesAsync("Cluedo")).ToList();
    }



}

public class Benchmarker
{
    public static void Main()
    {
        var summary = BenchmarkRunner.Run(typeof(Benchmarker).Assembly);
    }
}