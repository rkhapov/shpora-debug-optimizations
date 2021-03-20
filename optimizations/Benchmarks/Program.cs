using BenchmarkDotNet.Running;
using Benchmarks.Benchmarks;

namespace Benchmarks
{
	class Program
	{
		static void Main(string[] args)
		{
			BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
			//BenchmarkRunner.Run<MemoryTraffic>();
			//BenchmarkRunner.Run<StructVsClassBenchmark>();
			//BenchmarkRunner.Run<ByteArrayEqualityBenchmark>();
			//BenchmarkRunner.Run<NewConstraintBenchmark>();
			//BenchmarkRunner.Run<MaxBenchmark>();
		}
	}
}
