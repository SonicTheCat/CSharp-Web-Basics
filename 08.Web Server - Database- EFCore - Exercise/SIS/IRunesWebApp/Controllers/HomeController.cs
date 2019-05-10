namespace IRunesWebApp.Controllers
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                this.ViewBag["username"] = request.Session.GetParameter("username").ToString();  
                return this.LoggedIn(request);
            }

            return this.View(); 
        }

        public IHttpResponse LoggedIn(IHttpRequest request)
        {
            return this.View(); 
        }
    }
}