using CakeWebApp.Controllers;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;

namespace CakeWebApp
{
    public class Program
    {
        static void Main()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            //GET
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/home/index"] = request => new RedirectResult("/");
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/login"] = request => new UsersController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/register"] = request => new UsersController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/logout"] = request => new UsersController().Logout(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/profile"] = request => new UsersController().Profile(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/products/add"] = request => new ProductsController().Add(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/products/all"] = request => new ProductsController().All(request);

            //POST
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/login"] = request => new UsersController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/register"] = request => new UsersController().DoRegister(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/products/add"] = request => new ProductsController().AddPostRequest(request);

            Server server = new Server(80, serverRoutingTable);
            server.Run();
        }
    }
}