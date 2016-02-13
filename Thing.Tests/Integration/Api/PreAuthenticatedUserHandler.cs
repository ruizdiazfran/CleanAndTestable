using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Thing.Tests.Integration.Api
{
    public class PreAuthenticatedUserHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.GetRequestContext().Principal = new ClaimsPrincipal(new GenericIdentity("TestUser"));
            return base.SendAsync(request, cancellationToken);
        }
    }
}