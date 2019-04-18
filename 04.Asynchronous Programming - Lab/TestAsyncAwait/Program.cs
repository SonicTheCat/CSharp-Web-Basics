using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsyncAwait
{
    public class Program
    {
        public static void Main()
        {
            var task = PrintNumbersAsync();
            Console.WriteLine("Hello from Main!");

            //More dummy code here...
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                Console.Write(i + ", ");
            }
            
            Console.WriteLine("Goodbye from Main!");

            task.Wait(); 
        }

        private static async Task PrintNumbersAsync()
        {
            Console.WriteLine("PrintNumbersMethod Says Hello!");
            var counter = await Task.Run(() => SlowOperations());
            Console.WriteLine($"Counter = {counter}, Goodbye from PrintNumbersMethod!");
        }

        private static int SlowOperations()
        {
            Console.WriteLine("SlowOperationsMethod says Hello!");
            var nums = 0;
            for (int i = 0; i < 1000_000_000; i++)
            {
                nums++;
            }

            Console.WriteLine("Goodbye from SlowOperationsMethod!");
            return nums;
        }
    }
}