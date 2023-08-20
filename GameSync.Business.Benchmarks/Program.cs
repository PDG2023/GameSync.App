using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using GameSync.Business.BoardGamesGeek;

[SimpleJob(RuntimeMoniker.Net70)]
[MaxIterationCount(16)]
public class BggClient
{

    private readonly BoardGameGeekClient _client = new();
    private readonly IEnumerable<int> ids = Enumerable.Range(1000, 1300);

    [Benchmark]
    public async Task GetDetails()
    {
        await _client.GetBoardGamesDetailAsync(ids);
    }
}

public class Program
{
    public static void Main()
    {
        var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}