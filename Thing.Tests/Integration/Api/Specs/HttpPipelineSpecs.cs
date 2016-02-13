using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Should;
using Thing.Core.Contracts;
using Autofac;

namespace Thing.Tests.Integration.Api.Specs
{
    public class HttpPipelineSpecs
    {
        public void Should_resolve_service_from_request()
        {
            //  Arrange
            Action<HttpRequestMessage> assert = _ => _.GetDependencyScope().GetService(typeof(IUnitOfWork)).ShouldNotBeNull();
            var httpClient = new HttpConfiguration().ToHttpClient(new InspectHttpRequestMessageHandler(assert));

            //  Act
            var response = httpClient.GetAsync("/foo").Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            httpClient.Dispose();
        }

        public void Should_get_same_service_from_owin_context_and_request()
        {
            //  Arrange
            Action<HttpRequestMessage> assert = _ => 
                _.GetOwinContext().Get<ILifetimeScope>("autofac:OwinLifetimeScope").Resolve<IUnitOfWork>()
                .ShouldBeSameAs(_.GetDependencyScope().GetService(typeof(IUnitOfWork)));

            var httpClient = new HttpConfiguration().ToHttpClient(new InspectHttpRequestMessageHandler(assert));

            //  Act
            var response = httpClient.GetAsync("/foo").Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            httpClient.Dispose();
        }

        public void Should_resolve_service_from_owin_context()
        {
            //  Arrange
            Action<HttpRequestMessage> assert = _ => _.GetOwinContext().Get<ILifetimeScope>("autofac:OwinLifetimeScope").Resolve<IUnitOfWork>().ShouldNotBeNull();
            var httpClient = new HttpConfiguration().ToHttpClient(new InspectHttpRequestMessageHandler(assert));

            //  Act
            var response = httpClient.GetAsync("/foo").Result;

            //  Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            httpClient.Dispose();
        }
    }
}