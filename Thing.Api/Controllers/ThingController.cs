using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Thing.Core.Contracts;
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

        [Route("{id}", Name = "ThingDetail")]
        public async Task<IHttpActionResult> Get([FromUri] ThingQuery.GetById input)
        {
            try
            {
                var result = await _mediator.SendAsync(input);

                return Ok(result);
            }
            catch (EntityNotFound)
            {
                return NotFound();
            }
        }

        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] ThingCommand.Create input)
        {
            await _mediator.SendAsync(input);

            return CreatedAtRoute("ThingDetail",new {id= input.Id},input);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri] ThingCommand.Delete input)
        {
            try
            {
                await _mediator.SendAsync(input);
            }
            catch (EntityNotFound)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
