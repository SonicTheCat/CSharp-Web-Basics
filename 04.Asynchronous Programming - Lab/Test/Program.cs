using System;
using System.Collections.Generic;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main()
        {

            #region TestLock

            var threads = new List<Thread>();
            var num = 0;
            var objToLock = new object();

            for (int i = 0; i < 4; i++)
            {
                var thread = new Thread(() =>
                {
                    for (int j = 0; j < 1_000_000_000; j++)
                    {
                        //    lock (objToLock)
                        {
                            num++;
                        }
                    }

                    //Console.WriteLine("FINISHED!");
                    //Console.WriteLine(num);
                });

                // threads.Add(thread);
                thread.Start();
                Console.WriteLine("A new thread has started!");
            }

            //foreach (var thread in threads)
            //{
            //    thread.Join();
            //}

            Console.WriteLine(num);

            #endregion

            #region TestOne
            //List<int> listOfNums = new List<int>();

            ////Thread thread = new Thread(() =>
            ////{
            ////   var result = AddNumsAtFirstIndex(listOfNums, 10, 2_00_000);
            ////    Console.WriteLine("ID = " + Thread.CurrentThread.ManagedThreadId + ", Result = " + result); 
            ////}); 

            ////thread.Start();

            //var numbersAdded = Task.Run(() => AddNumsAtFirstIndex(listOfNums, 10, 1_00_000));
            //numbersAdded.ContinueWith((task) => Console.WriteLine("ID = " + Thread.CurrentThread.ManagedThreadId + ", Result = " + task.Result));

            //string userInput;
            //while ((userInput = Console.ReadLine()) != "Exit")
            //{
            //    Console.WriteLine(userInput);
            //}

            //// numbersAdded.Wait(); 

            #endregion
        }

        static string AddNumsAtFirstIndex(List<int> listOfNums, int min, int max)
        {
            for (int i = min; i <= max; i++)
            {
                listOfNums.Insert(0, i);
            }

            return (max - min) + " numbers added";
        }
    }
}
