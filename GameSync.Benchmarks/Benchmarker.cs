using BenchmarkDotNet.Running;

namespace GameSync.Benchmarks
{


    public class Benchmarker
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Benchmarker).Assembly).Run(args);

        }
    }
}