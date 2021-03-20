using System.IO;
using BenchmarkDotNet.Attributes;
using ImageProcessing;

namespace DotTraceExamples.Benchmarks
{
	public class EdgePreservingSmoothingBenchmark
	{
		private RGBImage image;

		[GlobalSetup]
		public void Setup()
		{
			var fileName = @"TestImages\TestImage.jpg";
			using (var fileStream = File.OpenRead(fileName))
			{
				image = RGBImage.FromStream(fileStream);
			}
		}

		[Benchmark]
		public void EdgePreservingSmoothing()
		{
			image.EdgePreservingSmoothing();
		}
	}
}