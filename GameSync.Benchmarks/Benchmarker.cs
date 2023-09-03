using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using GameSync.Api.BoardGameGeek;
using GameSync.Api.CommonResponses;
using Microsoft.Extensions.Caching.Memory;


[SimpleJob(RuntimeMoniker.Net70)]
[IterationCount(5)]
public class BggClient
{

    private static BoardGameGeekClient _noCacheClient = new();
    private static CachedBoardGameGeekClient _cachedClient = new(new MemoryCache(new MemoryCacheOptions()));
    private static IEnumerable<int> ids = Enumerable.Range(1000, 1200).ToList();


    [Benchmark]
    public async Task<List<GameDetail>> GetDetailsNoCache()
    {
        return (await _noCacheClient.GetBoardGamesDetailAsync(ids)).ToList();
    }

    [Benchmark]
    public async Task<List<GameDetail>> GetDetailsCached()
    {
        return (await _cachedClient.GetBoardGamesDetailAsync(ids)).ToList();
    }
    

    [Benchmark]
    public async Task<List<GamePreview>> SearchCluedoNoCache()
    {
        return (await _noCacheClient.SearchBoardGamesAsync("Clu")).ToList();
    }

    [Benchmark]
    public async Task<List<GamePreview>> SearchCluedoCache()
    {
        return (await _cachedClient.SearchBoardGamesAsync("Clu")).ToList();
    }



}

public class Benchmarker
{
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Benchmarker).Assembly).Run(args);
    }
}