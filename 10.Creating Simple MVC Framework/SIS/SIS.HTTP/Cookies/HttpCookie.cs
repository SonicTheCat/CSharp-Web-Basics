namespace SIS.HTTP.Cookies
{
    using System;

    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationDays = 3;

        public HttpCookie(string key, string value, int expires = HttpCookieDefaultExpirationDays)
        {
            this.Key = key;
            this.Value = value;
            this.Expires = DateTime.UtcNow.AddDays(expires); 
        }

        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpirationDays)
            :this(key, value, expires)
        {
            this.IsNew = isNew; 
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime Expires { get; private set; }

        public bool IsNew { get; set; }

        public bool IsHttpOnly { get; set; } = true; 

        public void Delete()
        {
            this.Expires = DateTime.UtcNow.AddDays(-1); 
        }

        public override string ToString()
        {
            var result = $"{this.Key}={this.Value}; Expires={this.Expires.ToString("R")}";
            if (this.IsHttpOnly)
            {
                result += "; HttpOnly"; 
            }

            return result; 
        }
    }
}