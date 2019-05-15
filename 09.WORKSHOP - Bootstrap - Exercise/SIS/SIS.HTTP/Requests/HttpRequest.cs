namespace SIS.HTTP.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;
    using Enums;
    using Exceptions;
    using Headers;
    using Contracts;
    using SIS.HTTP.Cookies.Contracts;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Sessions.Contracts;

    public class HttpRequest : IHttpRequest
    {
        private const string CookieWord = "Cookie";

        private const string CookieSplitDelimetar = "; ";

        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpSession Session { get; set; }

        private void ParseRequest(string requestString)
        {
            var splitRequestContent = requestString
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var requestLine = splitRequestContent[0]
                .Trim()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();
            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }

        private void ParseCookies()
        {
            if (!this.Headers.ContainsHeader(CookieWord))
            {
                return;
            }

            var cookiesString = this.Headers.GetHeader(CookieWord).Value;
            if (string.IsNullOrEmpty(cookiesString))
            {
                return;
            }

            var cookies = cookiesString.Split(CookieSplitDelimetar, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cookie in cookies)
            {
                var cookieKeyValue = cookie.Split("=", 2, StringSplitOptions.RemoveEmptyEntries);
                if (cookieKeyValue.Length != 2)
                {
                    continue; 
                }

                this.Cookies.Add(new HttpCookie(cookieKeyValue[0], cookieKeyValue[1], false));
            }
        }

        private void ParseRequestParameters(string formData)
        {
            this.ParseQueryParameters();
            this.ParseFormDataParameters(formData);
        }

        private void ParseFormDataParameters(string formData)
        {
            if (string.IsNullOrEmpty(formData))
            {
                return;
            }

            var formDataParams = formData.Split('&');

            foreach (var formDataParam in formDataParams)
            {
                string[] parameterArguments = formDataParam.Split('=');
                if (parameterArguments.Length != 2)
                {
                    throw new BadRequestException();
                }

                this.FormData[parameterArguments[0]] = parameterArguments[1];
            }
        }

        private void ParseQueryParameters()
        {
            if (!this.Url.Contains('?'))
            {
                return;
            }

            var queryString = this.Url.Split('?', '#', StringSplitOptions.None)[1];

            if (string.IsNullOrWhiteSpace(queryString))
            {
                return;
            }

            var queryParameters = queryString.Split('&');

            if (!this.IsValidRequestQueryString(queryString, queryParameters))
            {
                throw new BadRequestException();
            }

            foreach (var queryParameter in queryParameters)
            {
                var arguments = queryParameter.Split('=');
                this.QueryData[arguments[0]] = arguments[1];
            }
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            return !(string.IsNullOrEmpty(queryString) || queryParameters.Length < 1);
        }

        private void ParseHeaders(string[] requestHeaders)
        {
            if (!requestHeaders.Any())
            {
                throw new BadRequestException();
            }

            foreach (var header in requestHeaders)
            {
                if (string.IsNullOrEmpty(header))
                {
                    break;
                }

                var splitedReqHeader = header.Split(": ", StringSplitOptions.RemoveEmptyEntries);
                var reqHeaderKey = splitedReqHeader[0];
                var reqHeaderValue = splitedReqHeader[1];
                this.Headers.Add(new HttpHeader(reqHeaderKey, reqHeaderValue));
            }

            //TODO make it contains key 
            if (!this.Headers.ContainsHeader("Host"))
            {
                throw new BadRequestException();
            }
        }

        private void ParseRequestPath()
        {
            var path = this.Url.Split('?', '#')[0];
            if (string.IsNullOrEmpty(path))
            {
                throw new BadRequestException();
            }

            this.Path = path;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            var url = requestLine[1];
            if (string.IsNullOrEmpty(url))
            {
                throw new BadRequestException();
            }

            this.Url = url;
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            var isMethodValid = Enum.TryParse(typeof(HttpRequestMethod), requestLine[0], true, out object requestMethod);
            if (!isMethodValid)
            {
                throw new BadRequestException();
            }

            this.RequestMethod = (HttpRequestMethod)requestMethod;
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            return requestLine.Length == 3 && requestLine[2] == GlobalConstants.HttpOneProtocolFragment;
        }
    }
}