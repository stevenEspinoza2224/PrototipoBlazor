using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.Configuration
{
    public class Service
    {
        public string? Name { get; set; }

        public Uri? BaseUri { get; set; }

        public Credentials? Credentials { get; set; }

        public Endpoints? Endpoints { get; set; }
    }

    public class Credentials : List<Credential>
    {
        public Credential? this[string country] => this.FirstOrDefault(x => x.Country == country);

    }
    public class Credential
    {
        public string? Country { get; set; }

        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }

        public string? ResourceId { get; set; }

        public string? ApiKey { get; set; }
    }
    public class Endpoints : List<Endpoint>
    {
        public Endpoint? this[string name] => this.FirstOrDefault(x => x.Name == name);

    }
    public class Endpoint
    {
        public string? Method { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public TimeSpan Timeout { get; set; }

        public HttpMethod? HttpMethod => string.IsNullOrEmpty(Method) ? null : new HttpMethod(Method);
    }
}
