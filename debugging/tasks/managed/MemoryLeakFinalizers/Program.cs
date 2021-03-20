using System;
using System.Threading;

namespace MemoryLeakFinalizers
{
    internal class MyDataProcessor
    {
        private readonly byte[] data;

        public MyDataProcessor(byte[] data)
        {
            this.data = data;
        }

        public void DoSomeWork()
        {
            for (var i = 1; i < data.Length; i++)
            {
                data[i] = (byte) (data[i - 1] + 10);
            }
        }

        ~MyDataProcessor()
        {
            while (true)
            {
            }
        }
    }

    internal static class Program
    {
        private static readonly Random Random = new Random();

        private static void Main()
        {
            while (true)
            {
                var data = new byte[10 * 1024];
                Random.NextBytes(data);

                new MyDataProcessor(data).DoSomeWork();

                Thread.Sleep(5);
            }
        }
    }
}