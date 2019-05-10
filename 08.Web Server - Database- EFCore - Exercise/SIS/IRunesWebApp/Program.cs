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

        //POST
        private static void InitializePostMethods(ServerRoutingTable serverRoutingTable)
        {
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/register"] =
             request => new UsersController().RegisterPostRequest(request);

            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/login"] =
             request => new UsersController().LoginPostRequest(request);

            serverRoutingTable.Routes[HttpRequestMethod.Post]["/albums/create"] =
             request => new AlbumsController().CreateAlbumPostRequest(request);

            serverRoutingTable.Routes[HttpRequestMethod.Post]["/tracks/create"] =
             request => new TracksController().CreateTrackPostRequest(request);
        }

        //GET
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

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/all"] =
                request => new AlbumsController().All(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/create"] =
                request => new AlbumsController().Create(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/details"] =
                request => new AlbumsController().Details(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/create"] =
                request => new TracksController().Create(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/details"] =
                request => new TracksController().Details(request);
        }
    }
}