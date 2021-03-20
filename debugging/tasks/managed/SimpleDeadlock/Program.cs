using System;
using System.Threading;

namespace SimpleDeadlock
{
    internal static class Program
    {
        private static readonly object lock1 = new object();
        private static readonly object lock2 = new object();

        private static void Task1()
        {
            lock (lock1)
            {
                Thread.Sleep(1000);
                lock (lock2)
                {
                    Console.WriteLine("Ping");
                }
            }
        }

        private static void Task2()
        {
            lock (lock2)
            {
                Thread.Sleep(1000);
                lock (lock1)
                {
                    Console.WriteLine("Pong");
                }
            }
        }

        private static void Main()
        {
            Console.WriteLine("Here comes the ping pong:");
            
            var task1 = new Thread(Task1);
            var task2 = new Thread(Task2);

            task1.Start();
            task2.Start();

            task1.Join();
            task2.Join();
        }
    }
}