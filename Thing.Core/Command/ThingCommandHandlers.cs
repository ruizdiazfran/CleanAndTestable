using System.Threading.Tasks;
using MediatR;
using Thing.Core.Contracts;

namespace Thing.Core.Command
{
    public class ThingCommandHandlers : IAsyncRequestHandler<ThingCommand.Create, Unit>
    {
        private readonly IThingRepository _thingRepository;

        public ThingCommandHandlers(IThingRepository thingRepository)
        {
            _thingRepository = thingRepository;
        }

        public Task<Unit> Handle(ThingCommand.Create message)
        {
#pragma warning disable 618
            var entity = new Domain.Thing(message.Id, message.Name).SetAddress(message.AddressLine, message.AddressZip);
#pragma warning restore 618

            _thingRepository.Add(entity);

            return Task.FromResult(Unit.Value);
        }
    }
}