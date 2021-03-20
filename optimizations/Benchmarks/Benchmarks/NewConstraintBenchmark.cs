using System;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.Benchmarks
{
	[DisassemblyDiagnoser]
	public class NewConstraintBenchmark
	{
		[Benchmark]
		public object OperatorNew()
		{
			return CreateObject();
		}

		[Benchmark]
		public object DelegateFactory()
		{
			return CreateWithFactory(() => new object());
		}

		[Benchmark]
		public object NewGenericConstraint()
		{
			return Create<object>();
		}

		private static object CreateObject()
		{
			return new object();
		}

		private static T CreateWithFactory<T>(Func<T> factory)
		{
			return factory();
		}

		private static T Create<T>() where T : new()
		{
			return new T();
		}
	}
}