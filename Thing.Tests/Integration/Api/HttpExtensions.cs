using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Testing;

namespace Thing.Tests.Integration.Api
{
    public static class HttpExtensions
    {
        private static readonly Func<HttpConfiguration, TestServer> HttpHandlerFactory = (configuration) =>
        {
            return TestServer.Create(app => {
                new TestStartup(configuration).Configuration(app);
            });
        };

        public static HttpClient ToHttpClient(this HttpConfiguration configuration, string uri = "http://localhost")
        {
            var client = HttpHandlerFactory(configuration).HttpClient;
            client.BaseAddress = new Uri(uri);
            return client;
        }

        public static HttpClient ToHttpClient(this HttpConfiguration configuration, params DelegatingHandler[] handlers)
        {
            var client = HttpHandlerFactory(configuration.AddMessageHandlers(handlers)).HttpClient;
            return client;
        }

        private static HttpConfiguration AddMessageHandlers(this HttpConfiguration configuration,
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