using System;
using System.Threading;

namespace HarderDeadlock
{
    internal static class Program
    {
        private static readonly object myLock = new object();

        private static ulong Fib(ulong n)
        {
            var a = 0UL;
            var b = 1UL;

            for (var i = 0UL; i < n; i++)
            {
                var t = a + b;
                a = b;
                b = t;
            }

            return a;
        }

        private static ulong First10KFibsSum()
        {
            var sum = 1UL;
            const ulong total = 10000UL;

            for (var i = 0UL; i < total; i++)
            {
                sum += Fib(sum);
            }

            return sum;
        }

        private static void Task1()
        {
            lock (myLock)
            {
                Console.WriteLine("Will little adventure becomes a big trouble?\n" +
                                  $"Of course no, because sum of first 10k (={First10KFibsSum()}) fibs is not big enough");
            }
        }

        private static void Task2()
        {
            Thread.Sleep(1000);

            lock (myLock)
            {
                Console.WriteLine("Just wait for sum of first 10k fibs...");
            }
        }
        
        public static void Main()
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