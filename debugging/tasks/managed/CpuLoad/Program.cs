using System;
using System.Threading;

namespace CpuLoad
{
    internal static class Program
    {
        private const int N = 100000;
        private static readonly Random random = new Random();
        private static readonly int[] array1 = new int[N];
        private static readonly int[] array2 = new int[N];

        private static void SortByBubble()
        {
            for (var i = 0; i < N; i++)
            {
                array1[i] = random.Next();
            }

            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    if (array1[i] <= array1[j])
                        continue;

                    var t = array1[i];
                    array1[i] = array1[j];
                    array1[j] = t;
                }
            }
        }

        private static void SortByArraySort()
        {
            for (var i = 0; i < N; i++)
            {
                array2[i] = random.Next();
            }

            Array.Sort(array2);
        }

        private static void Task1()
        {
            while (true)
            {
                SortByBubble();
                Thread.Sleep(1000);
            }
        }

        private static void Task2()
        {
            while (true)
            {
                SortByArraySort();
                Thread.Sleep(1000);
            }
        }

        private static void Main()
        {
            var thread1 = new Thread(Task1);
            var thread2 = new Thread(Task2);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }
    }
}