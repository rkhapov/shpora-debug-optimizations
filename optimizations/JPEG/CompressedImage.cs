using System;
using System.Collections.Generic;
using System.IO;

namespace JPEG
{
	public class CompressedImage
	{
		public int Width { get; set; }
		public int Height { get; set; }

		public int Quality { get; set; }
		
		public Dictionary<BitsWithLength, byte> DecodeTable { get; set; }

		public long BitsCount { get; set; }
		public byte[] CompressedBytes { get; set; }

		public void Save(string path)
		{
			using(var sw = new FileStream(path, FileMode.Create))
			{
				byte[] buffer;

				buffer = BitConverter.GetBytes(Width);
				sw.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(Height);
				sw.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(Quality);
				sw.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(DecodeTable.Count);
				sw.Write(buffer, 0, buffer.Length);

				foreach(var kvp in DecodeTable)
				{
					var bits = kvp.Key.Bits;
					buffer = BitConverter.GetBytes(bits);
					sw.Write(buffer, 0, buffer.Length);

					var bitsCount = kvp.Key.BitsCount;
					buffer = BitConverter.GetBytes(bitsCount);
					sw.Write(buffer, 0, buffer.Length);

					var mappedByte = kvp.Value;
					sw.WriteByte(mappedByte);
				}

				buffer = BitConverter.GetBytes(BitsCount);
				sw.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(CompressedBytes.Length);
				sw.Write(buffer, 0, buffer.Length);

				sw.Write(CompressedBytes, 0, CompressedBytes.Length);
			}
		}

		public static CompressedImage Load(string path)
		{
			var result = new CompressedImage();
			using(var sr = new FileStream(path, FileMode.Open))
			{
				byte[] buffer = new byte[8];

				sr.Read(buffer, 0, 4);
				result.Width = BitConverter.ToInt32(buffer, 0);

				sr.Read(buffer, 0, 4);
				result.Height = BitConverter.ToInt32(buffer, 0);

				sr.Read(buffer, 0, 4);
				result.Quality = BitConverter.ToInt32(buffer, 0);

				sr.Read(buffer, 0, 4);
				var decodeTableSize = BitConverter.ToInt32(buffer, 0);
				result.DecodeTable = new Dictionary<BitsWithLength, byte>(decodeTableSize, new BitsWithLength.Comparer());

				for(int i = 0; i < decodeTableSize; i++)
				{
					sr.Read(buffer, 0, 4);
					var bits = BitConverter.ToInt32(buffer, 0);

					sr.Read(buffer, 0, 4);
					var bitsCount = BitConverter.ToInt32(buffer, 0);

					var mappedByte = (byte)sr.ReadByte();
					result.DecodeTable[new BitsWithLength {Bits = bits, BitsCount = bitsCount}] = mappedByte;
				}

				sr.Read(buffer, 0, 8);
				result.BitsCount = BitConverter.ToInt64(buffer, 0);

				sr.Read(buffer, 0, 4);
				var compressedBytesCount = BitConverter.ToInt32(buffer, 0);

				result.CompressedBytes = new byte[compressedBytesCount];
				var totalRead = 0;
				while(totalRead < compressedBytesCount)
					totalRead += sr.Read(result.CompressedBytes, totalRead, compressedBytesCount - totalRead);
			}
			return result;
		}
	}
}