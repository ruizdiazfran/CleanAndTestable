using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SampleLibrary.Query;

namespace Thing.Api.Controllers
{
    [RoutePrefix("api/thing")]
    public class ThingController : ApiController
    {
        private readonly IMediator _mediator;

        public ThingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var result = await _mediator.SendAsync(new Query.GetAll());

            return Ok(result);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var result = await _mediator.SendAsync(new Query.GetById {Id = id});

            return Ok(result);
        }
    }
}