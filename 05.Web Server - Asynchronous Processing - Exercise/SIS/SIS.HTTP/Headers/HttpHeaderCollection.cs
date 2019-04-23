namespace SIS.HTTP.Headers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            if (header == null ||
                string.IsNullOrEmpty(header.Key) ||
                string.IsNullOrEmpty(header.Value) ||
                this.ContainsHeader(header.Key))
            {
                throw new Exception();
            }

            this.headers[header.Key] = header;
        }

        public bool ContainsHeader(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception(); 
            }

            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            return this.ContainsHeader(key) ? this.headers[key] : null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var kvp in this.headers)
            {
                sb.AppendLine(kvp.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}