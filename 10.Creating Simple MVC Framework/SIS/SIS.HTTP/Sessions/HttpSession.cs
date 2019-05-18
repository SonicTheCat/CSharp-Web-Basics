namespace SIS.HTTP.Sessions
{
    using SIS.HTTP.Sessions.Contracts;
    using System;
    using System.Collections.Generic;

    public class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> parameters;

        public HttpSession(string id)
        {
            this.Id = id; 
            this.parameters = new Dictionary<string, object>();
        }

        public string Id {get; }

        public void AddParameter(string name, object parameter)
        {
            if (this.ContainsParameter(name))
            {
                throw new ArgumentException(); 
            }

            this.parameters[name] = parameter; 
        }

        public void ClearParameters()
        {
            this.parameters.Clear(); 
        }

        public bool ContainsParameter(string name)
        {
            return this.parameters.ContainsKey(name); 
        }

        public object GetParameter(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(); 
            }

            if (!this.ContainsParameter(key))
            {
                return null; 
            }

            return this.parameters[key]; 
        }
    }
}