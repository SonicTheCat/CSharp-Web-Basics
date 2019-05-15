namespace SIS.HTTP.Cookies
{
    using SIS.HTTP.Cookies.Contracts;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            if (cookie == null ||
              string.IsNullOrEmpty(cookie.Key) ||
              string.IsNullOrEmpty(cookie.Value))
            {
                throw new Exception();
            }

            this.cookies[cookie.Key] = cookie;
        }

        public bool ContainsCookie(string key)
        {
            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            if (!this.ContainsCookie(key))
            {
                return null;
            }

            return this.cookies[key];
        }

        public bool HasCookies()
        {
            return this.cookies.Any();
        }

        //TODO: dont need this ToString method 
        public override string ToString()
        {
            return string.Join("; ", this.cookies.Values);
        }

        IEnumerator<HttpCookie> IEnumerable<HttpCookie>.GetEnumerator()
        {
            foreach (var kvp in this.cookies)
            {
                yield return kvp.Value;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}