using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Builder;

namespace Thing.Tests.Integration.Api
{
    public static class HttpExtensions
    {
        private static readonly Func<HttpConfiguration, HttpMessageHandler> HttpHandlerFactory = configuration =>
        {
            var app = new AppBuilder();
            new TestStartup(configuration).Configuration(app);
            return new OwinHttpMessageHandler(app.Build());
        };

        public static HttpClient ToHttpClient(this HttpMessageHandler handler, string uri = "http://localhost")
        {
            return new HttpClient(handler)
            {
                BaseAddress = new Uri(uri)
            };
        }

        public static HttpClient ToHttpClient(this HttpConfiguration configuration, string uri = "http://localhost")
        {
            return HttpHandlerFactory(configuration).ToHttpClient(uri);
        }

        public static HttpClient ToHttpClient(this HttpConfiguration configuration, params DelegatingHandler[] handlers)
        {
            return HttpHandlerFactory(configuration.AddMessageHandlers(handlers)).ToHttpClient();
        }

        public static HttpConfiguration AddMessageHandlers(this HttpConfiguration configuration,
            params DelegatingHandler[] handlers)
        {
            foreach (var delegatingHandler in handlers)
            {
                configuration.MessageHandlers.Add(delegatingHandler);
            }

            return configuration;
        }
    }
}