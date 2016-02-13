using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Should;

namespace Thing.Tests.Integration.Api.Specs
{
    public class InfoSpecs : IDisposable
    {
        private readonly HttpClient _httpClient;

        public InfoSpecs(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public void Should_get()
        {
            //  Arrange

            //  Act
            var response = _httpClient.GetAsync("/api/info").Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
        }
    }
}