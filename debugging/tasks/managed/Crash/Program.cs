using System;

namespace Crash
{
    internal static class Program
    {
        private static ulong Fib(int n)
        {
            if (n == 0)
                return 0;

            if (n == 1)
                return 1;

            return Fib(n - 1) + Fib(n - 2);
        }
        
        private static void Main(string[] args)
        {
            Console.WriteLine(Fib(100000));
        }
    }
}