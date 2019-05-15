namespace CakeWebApp.Controllers
{
    using CakeWebApp.Extensions;
    using CakeWebApp.Models;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;

    public class ProductsController : BaseController
    {
        public IHttpResponse Add(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            return this.View(); 
        }

        public IHttpResponse AddPostRequest(IHttpRequest request)
        {
            var productName = request.FormData["productName"].ToString().Trim().UrlDecode();
            var price = decimal.Parse(request.FormData["price"].ToString());
            var url = request.FormData["url"].ToString().Trim().UrlDecode();

            //TODO: Validate data!

            var product = new Product()
            {
                Name = productName,
                Price = price,
                ImageUrl = url
            };

            this.Db.Products.Add(product);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return new BadRequestResult(e.Message, HttpResponseStatusCode.BadRequest);
            }

            return new RedirectResult("/products/all");
        }

        public IHttpResponse All(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            var allProducts = this.Db.Products.ToList();

            var content = string.Empty;
            foreach (var product in allProducts)
            {
                content += $"<div class=\"col-lg-3 col-md-3 col-sm-3 col-xs-3\">" +
                    $"<div class=\"thumbnail\"><img src=\"{product.ImageUrl}\" alt=\"...\" width=\"200\" height=\"200\">" +
                    $"<div class=\"caption\">" +
                    $"<h3>{product.Name}</h3>" +
                     $"<p>Price: ${product.Price}</p>" +
                     $"<p><a href=\"#\" class=\"btn btn-primary\" role=\"button\">Details</a></p>" +
                    $"</div>" + 
                    $"</div>" + 
                    $"</div>"; 
            }
            this.ViewBag["products"] = content;

            if (!allProducts.Any())
            {
                content = "<p>Nothing to show</p>"; 
            }

            return this.View();
        }
    }
}