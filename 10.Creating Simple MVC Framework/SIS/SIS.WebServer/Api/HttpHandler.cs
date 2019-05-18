namespace SIS.WebServer.Api
{
    using SIS.HTTP.Common;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Api.Contracts;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;
    using System.IO;
    using System.Linq;

    public class HttpHandler : IHttpHandler
    {
        private const string RootDirectoryRelativePath = "../../..";

        private readonly ServerRoutingTable serverRoutingTable; 

        public HttpHandler(ServerRoutingTable serverRoutingTable)
        {
            this.serverRoutingTable = serverRoutingTable; 
        }

        private string GetResourceExtension(string path)
        {
            return path.Substring(path.LastIndexOf("."));
        }

        private bool IsResourceRequest(IHttpRequest httpRequest)
        {
            var requestPath = httpRequest.Path;
            if (requestPath.Contains("."))
            {
                var extension = GetResourceExtension(requestPath);
                return GlobalConstants.ResourceExtensions.Contains(extension);
            }
            return false;
        }

        private IHttpResponse HandleRequestResponse(string path)
        {
            var indexOfStartNameOfResource = path.LastIndexOf('/');
            var extension = GetResourceExtension(path);

            var resourceName = path.Substring(indexOfStartNameOfResource);

            var resourcePath = RootDirectoryRelativePath +
                "/Resources" +
                $"/{extension.Substring(1)}" +
                resourceName;

            if (!File.Exists(resourcePath))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            var fileContent = File.ReadAllBytes(resourcePath);

            return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);
        }

        public IHttpResponse Handle(IHttpRequest httpRequest)
        {
            var isResourceRequest = this.IsResourceRequest(httpRequest);
            if (isResourceRequest)
            {
                return this.HandleRequestResponse(httpRequest.Path);
            }

            if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod)
                || !this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path.ToLower()))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            var func = this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path];
            return func.Invoke(httpRequest);
        }
    }
}