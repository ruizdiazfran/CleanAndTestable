using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Thing.Tests.Integration.Api
{
    public class InspectHttpRequestMessageHandler : DelegatingHandler
    {
        readonly Action<HttpRequestMessage> _assert;

        public InspectHttpRequestMessageHandler(Action<HttpRequestMessage> assert)
        {
            _assert = assert;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            _assert(request);

            return Task.FromResult( request.CreateResponse(HttpStatusCode.OK) );
        }
    }
}