using System;
using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace DotTraceExamples.Benchmarks
{
	[MemoryDiagnoser]
	public class ComplexArrayExtensionsBenchmark
	{
		private Complex[] data;
		private Complex[] dividedData;

		[GlobalSetup]
		public void Setup()
		{
			data = new Complex[13000000];
			dividedData = data.DivideByNumber(Math.PI);
		}

		[Benchmark]
		public void DivideByNumber()
		{
			data.DivideByNumber(Math.PI);
		}

		[Benchmark]
		public void SumModules()
		{
			dividedData.SumModules();
		}
	}
}