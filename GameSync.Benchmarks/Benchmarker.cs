using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using GameSync.Api.BoardGameGeek;
using GameSync.Api.CommonResponses;
using GameSync.Api.Persistence.Entities.Games;
using Microsoft.Extensions.Caching.Memory;

namespace GameSync.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net70)]
    [IterationCount(5)]
    public class BggClient
    {

        private static BoardGameGeekClient _noCacheClient = new();
        private static CachedBoardGameGeekClient _cachedClient = new(new MemoryCache(new MemoryCacheOptions()));
        private static readonly IEnumerable<int> _ids = Enumerable.Range(1000, 1200).ToList();


        [Benchmark]
        public async Task<List<IGame>> GetDetailsNoCache()
        {
            return (await _noCacheClient.GetBoardGamesDetailAsync(_ids)).ToList();
        }

        [Benchmark]
        public async Task<List<IGame>> GetDetailsCached()
        {
            return (await _cachedClient.GetBoardGamesDetailAsync(_ids)).ToList();
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
}