using System;
using System.Net;
using System.Net.Http;
using Should;
using Thing.Core.Command;
using Thing.Core.Query;

namespace Thing.Tests.Integration.Api
{
    public class ThingApiSpecs : IDisposable
    {
        private readonly HttpClient _httpClient;

        public ThingApiSpecs(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public void Should_get_all(ThingQuery.GetAll request)
        {
            //  Arrange

            //  Act
            var response = _httpClient.GetAsync("/api/thing").Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }

        public void Should_get_one()
        {
            //  Arrange
            const string id = "my-first";

            //  Act
            var response = _httpClient.GetAsync($"/api/thing/{id}").Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }

        public void Should_create(ThingCommand.Create request)
        {
            //  Act

            //  Arrange
            var response = _httpClient.PostAsJsonAsync($"/api/thing",request).Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.Created);
            response.Headers.Location.AbsoluteUri.ShouldEqual($"http://localhost/api/thing/{request.Id}");
        }

    }
}