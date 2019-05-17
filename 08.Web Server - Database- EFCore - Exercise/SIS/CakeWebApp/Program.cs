using CakeWebApp.Controllers;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Api;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Routing;
using System;

namespace CakeWebApp
{
    public class Program
    {
        static void Main()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            IHttpHandler httpHandler = new HttpHandler(serverRoutingTable);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/login"] = request => new AccountController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/register"] = request => new AccountController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/register"] = request => new AccountController().DoRegistration(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/login"] = request => new AccountController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/profile"] = request => new AccountController().ShowProfile(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/add"] = request => new ProductController().Add(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/add"] = request => new ProductController().TryAddCake(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/search"] = request => new ProductController().Search(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/logout"] = request => new AccountController().Logout(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/hello"] = request => new HomeController().Hello(request);

            Server server = new Server(80, httpHandler);
            server.Run();
        }
    }
}