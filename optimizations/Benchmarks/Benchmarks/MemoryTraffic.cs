using System.Text;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.Benchmarks
{
	[MemoryDiagnoser]
	public class MemoryTraffic
	{
		private const int N = 5000;
		[Benchmark]
		public string StringBuilder()
		{
			var sb = new StringBuilder();
			for (var i = 0; i < N; ++i)
				sb.Append("abc");
			return sb.ToString();
		}

		[Benchmark]
		public string String()
		{
			var s = string.Empty;
			for (var i = 0; i < N; ++i)
				s += "abc";
			return s;
		}
	}
}