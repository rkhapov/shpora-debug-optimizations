using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.Benchmarks
{
	[DisassemblyDiagnoser]
	public class ByteArrayEqualityBenchmark
	{
		private byte[] aArr;
		private byte[] bArr;

		[GlobalSetup]
		public void Setup()
		{
			var random = new Random(42);
			aArr = new byte[100255];
			bArr = new byte[100255];
			random.NextBytes(aArr);
			Array.Copy(aArr, bArr, aArr.Length);
			bArr[bArr.Length - 1] = (byte)(bArr[bArr.Length - 1] + 1);
		}

		[Benchmark]
		public bool Linq() => LinqCompare(aArr, bArr);

		[Benchmark]
		public bool Trivial() => TrivialCompare(aArr, bArr);

		[Benchmark]
		public bool ReadOnly() => TrivialCompareIReadOnly(aArr, bArr);

		[Benchmark]
		public bool Structural() => StructuralCompare(aArr, bArr);

		[Benchmark]
		public bool Unrolled() => TrivialCompareUnrolled(aArr, bArr);

		[Benchmark]
		public bool Long() => LongCompare(aArr, bArr);

		[Benchmark]
		public bool Guid() => GuidCompare(aArr, bArr);

		[Benchmark]
		public bool Span() => SpanCompare(aArr, bArr);

		[Benchmark]
		public bool Vectors() => VectorCompare(aArr, bArr);

		[Benchmark]
		public bool SequenceEqual() => SequenceCompare(aArr, bArr);

		private static bool SequenceCompare(byte[] x, byte[] y) => x.SequenceEqual(y);

		[Benchmark]
		public bool Native() => NativeCompare(aArr, bArr);

		private static bool TrivialCompare(byte[] x, byte[] y)
		{
			if (x.Length != y.Length)
				return false;
			for (var i = 0; i < x.Length; ++i)
				if (x[i] != y[i])
					return false;

			return true;
		}

		private static bool TrivialCompareIReadOnly(IReadOnlyList<byte> x, IReadOnlyList<byte> y)
		{
			if (x.Count != y.Count)
				return false;
			for (var i = 0; i < x.Count; ++i)
				if (x[i] != y[i])
					return false;

			return true;
		}

		private static bool LinqCompare(IReadOnlyCollection<byte> x, IReadOnlyCollection<byte> y)
		{
			return x.Count == y.Count &&
				   x.Zip(y, (a, b) => a - b)
					.All(a => a == 0);
		}

		private static bool StructuralCompare(byte[] x, byte[] y)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
		}

		private static bool TrivialCompareUnrolled(byte[] x, byte[] y)
		{
			if (x.Length != y.Length)
				return false;
			var i = 0;
			var length = x.Length - x.Length % 8;
			while (i < length)
			{
				if (
					x[i] != y[i] ||
					x[i + 1] != y[i + 1] ||
					x[i + 2] != y[i + 2] ||
					x[i + 3] != y[i + 3] ||
					x[i + 4] != y[i + 4] ||
					x[i + 5] != y[i + 5] ||
					x[i + 6] != y[i + 6] ||
					x[i + 7] != y[i + 7])
					return false;
				i += 8;
			}
			for (; i < x.Length; ++i)
				if (x[i] != y[i])
					return false;

			return true;
		}

		public static unsafe bool LongCompare(byte[] x, byte[] y)
		{
			if (x.Length != y.Length)
				return false;
			var size = sizeof(long);
			var N = x.Length - x.Length % size;
			fixed (byte* p1 = x, p2 = y)
			{
				byte* x1 = p1, x2 = p2;
				for (var i = 0; i < N; ++i, x1 += size, x2 += size)
					if (*(long*)x1 != *(long*)x2)
						return false;
				for (var i = 0; i < x.Length % size; ++i, ++x1, ++x2)
					if (*x1 != *x2)
						return false;
				return true;
			}
		}

		public static unsafe bool GuidCompare(byte[] x, byte[] y)
		{
			var size = sizeof(Guid);

			if (x.Length != y.Length)
				return false;

			var N = x.Length - x.Length % size;
			fixed (byte* p1 = x, p2 = y)
			{
				byte* x1 = p1, x2 = p2;
				for (var i = 0; i < N; ++i, x1 += size, x2 += size)
					if (*(Guid*)x1 != *(Guid*)x2)
						return false;
				for (var i = 0; i < x.Length % size; ++i, ++x1, ++x2)
					if (*x1 != *x2)
						return false;
				return true;
			}
		}

		public static bool SpanCompare(byte[] x, byte[] y)
		{
			return x.AsSpan().SequenceCompareTo(y) == 0;
		}

		public static bool VectorCompare(byte[] x, byte[] y)
		{
			var offset = 0;
			for (var i = 0; i < x.Length / Vector<byte>.Count; ++i, offset += Vector<byte>.Count)
			{
				var a = new Vector<byte>(x, offset);
				var b = new Vector<byte>(y, offset);
				if (!Vector.EqualsAll(a, b))
					return false;
			}

			for (var i = 0; i < x.Length % Vector<byte>.Count; i++, offset++)
				if (x[offset] != y[offset])
					return false;

			return true;
		}

		public static unsafe bool NativeCompare(byte[] x, byte[] y)
		{
			fixed (byte* a = x)
			fixed (byte* b = y)
				return memcmp(a, b, (UIntPtr)x.Length) == 0;
		}

		[DllImport("msvcrt.dll")]
		public static extern unsafe int memcmp(byte* a, byte* b, UIntPtr length);
	}
}