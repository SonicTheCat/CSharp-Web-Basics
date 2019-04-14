namespace SoftUniHttpServer
{
    public class Startup
    {
        public static void Main()
        {
            IHttpServer httpServer = new HttpServer();
           
            httpServer.Start(); 
        }
    }
}