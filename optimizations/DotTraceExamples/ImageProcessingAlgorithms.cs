using System;
using ImageProcessing;

namespace DotTraceExamples
{
	public static class ImageProcessingAlgorithms
	{
		// Multiplication factor for convolution mask in edge preserving smoothing
		private const int multiplicationFactor = 10;

		public static RGBImage EdgePreservingSmoothing(this RGBImage image)
		{
			var imageData = image.ImageData;
			var height = image.Height;
			var bytesPerLine = image.BytesPerLine;
			var bitesPerPixel = RGBImage.BytesPerPixel;
			var filteringData = new byte[imageData.Length];

			for (var y = 0; y < height; y++)
			{
				for (var x = 0; x < bytesPerLine; x += bitesPerPixel)
				{
					var i = y * bytesPerLine + x;

					if (x == 0 || y == 0 || x == bytesPerLine - bitesPerPixel || y == height - 1)
					{
						filteringData[i] = imageData[i];
						filteringData[i + 1] = imageData[i + 1];
						filteringData[i + 2] = imageData[i + 2];

						continue;
					}

					//Center pixel of convolution mask
					var centerRed = imageData[i + 2];
					var centerGreen = imageData[i + 1];
					var centerBlue = imageData[i];

					// Indexes of neighbor pixels of convolution mask
					var id1 = (y - 1) * bytesPerLine + (x - bitesPerPixel);
					var id2 = (y - 1) * bytesPerLine + x;
					var id3 = (y - 1) * bytesPerLine + x + bitesPerPixel;
					var id4 = y * bytesPerLine + (x - bitesPerPixel);
					var id5 = y * bytesPerLine + x + bitesPerPixel;
					var id6 = (y + 1) * bytesPerLine + (x - bitesPerPixel);
					var id7 = (y + 1) * bytesPerLine + x;
					var id8 = (y + 1) * bytesPerLine + x + bitesPerPixel;

					var c1 = GetCoefficient(centerRed, centerGreen, centerBlue, imageData[id1 + 2], imageData[id1 + 1],
						imageData[id1]);
					var c2 = GetCoefficient(centerRed, centerGreen, centerBlue, imageData[id2 + 2], imageData[id2 + 1],
						imageData[id2]);
					var c3 = GetCoefficient(centerRed, centerGreen, centerBlue, imageData[id3 + 2], imageData[id3 + 1],
						imageData[id3]);
					var c4 = GetCoefficient(centerRed, centerGreen, centerBlue, imageData[id4 + 2], imageData[id4 + 1],
						imageData[id4]);
					var c5 = GetCoefficient(centerRed, centerGreen, centerBlue, imageData[id5 + 2], imageData[id5 + 1],
						imageData[id5]);
					var c6 = GetCoefficient(centerRed, centerGreen, centerBlue, imageData[id6 + 2], imageData[id6 + 1],
						imageData[id6]);
					var c7 = GetCoefficient(centerRed, centerGreen, centerBlue, imageData[id7 + 2], imageData[id7 + 1],
						imageData[id7]);
					var c8 = GetCoefficient(centerRed, centerGreen, centerBlue, imageData[id8 + 2], imageData[id8 + 1],
						imageData[id8]);

					var csum = c1 + c2 + c3 + c4 + c5 + c6 + c7 + c8;

					var resultRed = (imageData[id1 + 2] * c8 +
									 imageData[id2 + 2] * c7 +
									 imageData[id3 + 2] * c6 +
									 imageData[id4 + 2] * c5 +
									 imageData[id5 + 2] * c4 +
									 imageData[id6 + 2] * c3 +
									 imageData[id7 + 2] * c2 +
									 imageData[id8 + 2] * c1) / csum;

					var resultGreen = (imageData[id1 + 1] * c8 +
									   imageData[id2 + 1] * c7 +
									   imageData[id3 + 1] * c6 +
									   imageData[id4 + 1] * c5 +
									   imageData[id5 + 1] * c4 +
									   imageData[id6 + 1] * c3 +
									   imageData[id7 + 1] * c2 +
									   imageData[id8 + 1] * c1) / csum;

					var resultBlue = (imageData[id1] * c8 +
									  imageData[id2] * c7 +
									  imageData[id3] * c6 +
									  imageData[id4] * c5 +
									  imageData[id5] * c4 +
									  imageData[id6] * c3 +
									  imageData[id7] * c2 +
									  imageData[id8] * c1) / csum;


					filteringData[i + 2] = (byte)resultRed;
					filteringData[i + 1] = (byte)resultGreen;
					filteringData[i] = (byte)resultBlue;
				}
			}

			return new RGBImage(image.Width, height, bytesPerLine, filteringData);
		}

		/// <summary>
		/// Calculate coefficient for the convolution mask of edge preserving smoothing
		/// </summary>
		/// <param name="centerRed"></param>
		/// <param name="centerGreen"></param>
		/// <param name="centerBlue"></param>
		/// <param name="neighborRed"></param>
		/// <param name="neighborGreen"></param>
		/// <param name="neighborBlue"></param>
		/// <returns></returns>
		private static double GetCoefficient(byte centerRed, byte centerGreen, byte centerBlue, byte neighborRed,
			byte neighborGreen, byte neighborBlue)
		{
			var d = Math.Abs(centerRed - neighborRed) + Math.Abs(centerGreen - neighborGreen) +
					Math.Abs(centerBlue - neighborBlue);

			return Math.Pow(1 - d, multiplicationFactor);
		}

	}
}