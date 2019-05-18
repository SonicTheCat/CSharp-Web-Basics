namespace DemoMvcFramework
{
    using SIS.Framework;
    using SIS.Framework.Routers;
    using SIS.WebServer;

    public class Launcher
    {
        public static void Main()
        {
            var server = new Server(80, new ControllerRouter());
            MvcEngine.Run(server); 
        }
    }
}