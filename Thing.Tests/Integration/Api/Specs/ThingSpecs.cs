using System;
using System.Net;
using System.Net.Http;
using Should;
using Thing.Core.Command;
using Thing.Core.Query;

namespace Thing.Tests.Integration.Api.Specs
{
    public class ThingSpecs : IDisposable
    {
        private readonly HttpClient _httpClient;

        public ThingSpecs(HttpClient httpClient)
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
            //  Arrange            

            //  Act
            var response = _httpClient.PostAsJsonAsync($"/api/thing", request).Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.Created);
            response.Headers.Location.AbsoluteUri.ShouldEqual($"http://localhost/api/thing/{request.Id}");
        }

        public void Should_delete(ThingCommand.Delete request)
        {
            //  Arrange
            request.Id = "my-thirdy";

            //  Act
            var response = _httpClient.DeleteAsync($"/api/thing/{request.Id}").Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }

        public void Should_not_delete_if_id_not_exists(ThingCommand.Delete request)
        {
            //  Arrange

            //  Act
            var response = _httpClient.DeleteAsync($"/api/thing/{request.Id}").Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
        }
    }
}