using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Thing.Tests.Integration.Api
{
    public class PreAuthenticatedUser : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var h = InnerHandler;
            request.GetRequestContext().Principal = new ClaimsPrincipal(new GenericIdentity("Test"));
            return base.SendAsync(request, cancellationToken);
        }
    }
}