using CakeWebApp.Common;
using CakeWebApp.Models;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Linq;

namespace CakeWebApp.Controllers
{
    public class ProductController : BaseController
    {
        private const string AddViewName = "Add";

        public IHttpResponse Add(IHttpRequest request)
        {
            return this.View(AddViewName);
        }

        public IHttpResponse TryAddCake(IHttpRequest request)
        {
            var cookie = request.Cookies.GetCookie(GlobalConstants.AuthCookieKeyName);

            if (cookie == null)
            {
                return new RedirectResult("/login");
            }

            var productName = request.FormData["name"].ToString().Trim();
            var price = decimal.Parse(request.FormData["price"].ToString());
            var pictureUrl = request.FormData["pictureUrl"].ToString();

            if (string.IsNullOrWhiteSpace(productName) || productName.Length < 3)
            {
                return this.BadRequestError("Invalid Product Name.");
            }

            if (price < 0)
            {
                return this.BadRequestError("Price can not be negative.");
            }

            var product = new Product()
            {
                Name = productName,
                Price = price,
                ImageUrl = pictureUrl
            };

            this.Db.Products.Add(product);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return new RedirectResult("/");
        }

        public IHttpResponse Search(IHttpRequest request)
        {
            var cookie = request.Cookies.GetCookie(GlobalConstants.AuthCookieKeyName);

            if (cookie == null)
            {
                return new RedirectResult("/login");
            }

            var content = "<p><a href=\"/\">Back to home</a></p><br />" +
                "<form><input name=\"serached\" type=\"text\" placeholder=\"ex chocolate cake\" />" +
                "<input name=\"search\" type=\"submit\" value=\"Search\" />" +
                "<br /></form>";
           
            var products = this.Db.Products.ToList();

            if (products.Count == 0)
            {
                content += "<p>No products found!</p>";
            }
            else
            {
                foreach (var product in products)
                {
                    content += $"<p><a href=\"#\">- {product.Name}</a> ${product.Price}</p><br />"; 
                }
            }

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}