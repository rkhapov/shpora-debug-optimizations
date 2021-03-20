using System.IO;
using ImageProcessing;

namespace DotTraceExamples.Programs
{
	public class EdgePreservingSmoothingProgram : IProgram
	{
		public void Run()
		{
			var fileName = @"TestImages\TestImage.jpg";
			using (var fileStream = File.OpenRead(fileName))
			{
				var image = RGBImage.FromStream(fileStream);
				image.EdgePreservingSmoothing().SaveToFile(Path.Combine(Directory.GetCurrentDirectory(), "TestImages",
					$"{Path.GetFileNameWithoutExtension(fileName)}_Processed.jpg"));
			}
		}
	}
}