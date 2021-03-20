using System;
using System.Collections.Generic;
using System.Threading;

namespace MemoryLeakStatic
{
    internal static class Program
    {
        private static readonly Random random = new Random();

        public static void Main()
        {
            while (true)
            {
                var data = new byte[10 * 1024];
                random.NextBytes(data);

                new MyDataProcessor(data).DoSomeWork();

                Thread.Sleep(1);
            }
        }
    }

    internal class MyDataProcessor
    {
        private static List<MyDataProcessor> someInstances = new List<MyDataProcessor>();
        private readonly byte[] data;

        public MyDataProcessor(byte[] data)
        {
            someInstances.Add(this);
            this.data = data;
        }

        public void DoSomeWork()
        {
            for (var i = 1; i < data.Length; i++)
            {
                data[i] = (byte) (data[i - 1] + 10);
            }
        }
    }
}