using Autofac;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Thing.Core.Contracts;
using Should;
using System;
using System.Net;

namespace Thing.Tests.Integration.Api
{
    public class CheckDependecyHandler : DelegatingHandler
    {
        readonly Action<HttpRequestMessage> _assert;

        public CheckDependecyHandler(Action<HttpRequestMessage> assert)
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