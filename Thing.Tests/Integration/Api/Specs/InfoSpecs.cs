using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Should;

namespace Thing.Tests.Integration.Api.Specs
{
    public class InfoSpecs : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClientAuth;

        public InfoSpecs(HttpClient httpClient, HttpConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClientAuth = configuration.ToHttpClient(new PreAuthenticatedUserHandler());
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _httpClientAuth.Dispose();
        }

        public void Should_throw_unauthorized()
        {
            //  Arrange

            //  Act
            var response = _httpClient.GetAsync("/api/info").Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
        }

        public void Should_get()
        {
            //  Arrange

            //  Act
            var response = _httpClientAuth.GetAsync("/api/info").Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
            var result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            result["name"].ToString().ShouldEqual("TestUser");
        }
    }
}