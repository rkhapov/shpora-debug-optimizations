using System.Drawing;

namespace JPEG.Images
{
    class Matrix
    {
        public readonly Pixel[,] Pixels;
        public readonly int Height;
        public readonly int Width;
				
        public Matrix(int height, int width)
        {
            Height = height;
            Width = width;
			
            Pixels = new Pixel[height,width];
            for(var i = 0; i< height; ++i)
            for(var j = 0; j< width; ++j)
                Pixels[i, j] = new Pixel(0, 0, 0, PixelFormat.RGB);
        }

        public static explicit operator Matrix(Bitmap bmp)
        {
            var height = bmp.Height - bmp.Height % 8;
            var width = bmp.Width - bmp.Width % 8;
            var matrix = new Matrix(height, width);

            for(var j = 0; j < height; j++)
            {
                for(var i = 0; i < width; i++)
                {
                    var pixel = bmp.GetPixel(i, j);
                    matrix.Pixels[j, i] = new Pixel(pixel.R, pixel.G, pixel.B, PixelFormat.RGB);
                }
            }

            return matrix;
        }

        public static explicit operator Bitmap(Matrix matrix)
        {
            var bmp = new Bitmap(matrix.Width, matrix.Height);

            for(var j = 0; j < bmp.Height; j++)
            {
                for(var i = 0; i < bmp.Width; i++)
                {
                    var pixel = matrix.Pixels[j, i];
                    bmp.SetPixel(i, j, Color.FromArgb(ToByte(pixel.R), ToByte(pixel.G), ToByte(pixel.B)));
                }
            }

            return bmp;
        }

        public static int ToByte(double d)
        {
            var val = (int) d;
            if (val > byte.MaxValue)
                return byte.MaxValue;
            if (val < byte.MinValue)
                return byte.MinValue;
            return val;
        }
    }
}