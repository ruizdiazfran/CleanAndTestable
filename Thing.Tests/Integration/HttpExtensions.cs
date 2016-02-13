using System;
using System.Net.Http;
using Microsoft.Owin.Testing;

namespace Thing.Tests.Integration
{
    public static class HttpExtensions
    {
        public static HttpClient ToHttpClient(this HttpMessageHandler handler)
        {
            return new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost")
            };
        }
    }
}