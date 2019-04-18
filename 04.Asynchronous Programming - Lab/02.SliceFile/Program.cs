namespace _02.SliceFile
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main()
        {
            var parts = int.Parse(Console.ReadLine());
            var filePath = @"../../../../../sliceMe.mp4";
            var destination = "../../../VideoParts/";

            var task = SliceFileAsync(filePath, destination, parts);

            Console.WriteLine("Anything else?");
            string input;
            while ((input = Console.ReadLine()) != "exit")
            {

            }

            task.Wait(); 
        }

        //private static void SliceFileAsync(string sourceFile, string destinationDir, int parts)
        //{
        //    Task.Run(() =>
        //    {
        //        SliceVideo(sourceFile, destinationDir, parts);
        //    });
        //}

        private static async Task SliceFileAsync(string sourceFile, string destinationDir, int parts)
        {
            using (var reader = new FileStream(sourceFile, FileMode.Open))
            {
                var extension = sourceFile.Substring(sourceFile.LastIndexOf(".") + 1);
                var partsSize = (reader.Length / parts) + 1;
                int bufferSize = 4096;
              
                for (int i = 1; i <= parts; i++)
                {
                    if (destinationDir == "")
                    {
                        destinationDir = "./";
                    }

                    long currentPartSize = 0;
                    var currentPart = destinationDir + $"Part-{i}.{extension}";
                    using (var writer = new FileStream(currentPart, FileMode.Create))
                    {
                        var buffer = new byte[bufferSize];
                        while (currentPartSize <= partsSize)
                        {
                            int readBytesCount = await reader.ReadAsync(buffer, 0, bufferSize);
                            if (readBytesCount == 0)
                            {
                                break;
                            }

                            await writer.WriteAsync(buffer, 0, bufferSize);
                            currentPartSize += bufferSize;
                        }
                    }

                    Thread.Sleep(500); 
                    Console.WriteLine("Slice complete.");
                }
            }
        }
    }
}