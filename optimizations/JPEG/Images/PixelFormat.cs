namespace JPEG.Images
{
    public class PixelFormat
    {
        private string Format;

        private PixelFormat(string format)
        {
            Format = format;
        }

        public static PixelFormat RGB => new PixelFormat(nameof(RGB));
        public static PixelFormat YCbCr => new PixelFormat(nameof(YCbCr));

        protected bool Equals(PixelFormat other)
        {
            return string.Equals(Format, other.Format);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PixelFormat) obj);
        }

        public override int GetHashCode()
        {
            return (Format != null ? Format.GetHashCode() : 0);
        }

        public static bool operator==(PixelFormat a, PixelFormat b)
        {
            return a.Equals(b);
        }
		
        public static bool operator!=(PixelFormat a, PixelFormat b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            return Format;
        }
        
        ~PixelFormat()
        {
            Format = null;
        }
    }
}