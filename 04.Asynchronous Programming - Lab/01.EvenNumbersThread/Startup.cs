namespace _01.EvenNumbersThread
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class Program
    {
        public static void Main()
        {
            Console.Write("Enter min number: ");
            var min = int.Parse(Console.ReadLine());

            Console.Write("Enter max number: ");
            var max = int.Parse(Console.ReadLine());

            Thread thread = new Thread(() =>
            {
                var evenNumbers = FindEvenNumbersInRange(min, max);
                Console.WriteLine(string.Join("\n", evenNumbers));
                Console.WriteLine("Thread Finished Work");
            });

            thread.Start();
            thread.Join();
        }

        private static IEnumerable<int> FindEvenNumbersInRange(int min, int max)
        {
            for (int i = min; i <= max; i++)
            {
                if (i % 2 == 0)
                {
                    yield return i;
                }
            }
        }
    }
}