using SIS.HTTP.Enums;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System.IO;

namespace SIS.Demo
{
    public class LoginController
    {
        public IHttpResponse Login()
        {
            string content = File.ReadAllText(@"C:\Users\Petya\Desktop\SoftUniPavel\ActiveModuls\SIS\SIS.Demo\LoginPage.html");
            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}