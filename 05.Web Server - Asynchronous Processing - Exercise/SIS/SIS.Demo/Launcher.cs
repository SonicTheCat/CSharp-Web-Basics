namespace SIS.Demo
{
    using SIS.WebServer;
    using SIS.WebServer.Routing;
    
    public class Launcher
    {
        public static void Main()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            serverRoutingTable.Routes[HTTP.Enums.HttpRequestMethod.Get]["/"] = request => new HomeController().Index();
            serverRoutingTable.Routes[HTTP.Enums.HttpRequestMethod.Get]["/login"] = request => new LoginController().Login();

            Server server = new Server(80, serverRoutingTable);
            server.Run(); 
        }
    }
}