namespace IRunesWebApp
{
    using IRunesWebApp.Controllers;
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;
    
    public class Program
    {
        public static void Main()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            InitializeGetMethods(serverRoutingTable);
            InitializePostMethods(serverRoutingTable); 
            
            Server server = new Server(80, serverRoutingTable);
            server.Run();
        }

        private static void InitializePostMethods(ServerRoutingTable serverRoutingTable)
        {
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/register"] =
             request => new UsersController().RegisterPostRequest(request);

            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/login"] =
             request => new UsersController().LoginPostRequest(request);
        }

        private static void InitializeGetMethods(ServerRoutingTable serverRoutingTable)
        {
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] =
               request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/home/index"] =
                request => new RedirectResult("/");

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/login"] =
              request => new UsersController().Login(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/register"] =
              request => new UsersController().Register(request);
        }
    }
}