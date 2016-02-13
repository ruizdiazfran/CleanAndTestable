using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Thing.Api.Controllers
{
    [RoutePrefix("api/info")]
    public class InfoController : ApiController
    {
        [Route(""), Authorize]
        public IHttpActionResult Get()
        {
            var user = Request.GetOwinContext().Authentication.User;

            return Ok(new
            {
                user.Identity.Name,
                Claims = user.Claims.Select(_ => _.Value)
            });
        }
    }
}