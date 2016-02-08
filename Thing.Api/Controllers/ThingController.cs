using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Thing.Core.Command;
using Thing.Core.Query;

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
            var result = await _mediator.SendAsync(new ThingQuery.GetAll());

            return Ok(result);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> Get([FromUri] ThingQuery.GetById input)
        {
            var result = await _mediator.SendAsync(input);

            return Ok(result);
        }

        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] ThingCommand.Create input)
        {
            await _mediator.SendAsync(input);

            return Ok();
        }

        [Route("")]
        public async Task<IHttpActionResult> Post([FromUri] ThingCommand.Delete input)
        {
            await _mediator.SendAsync(input);

            return Ok();
        }
    }
}