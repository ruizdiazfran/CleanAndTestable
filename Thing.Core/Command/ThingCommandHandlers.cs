using System.Threading.Tasks;
using MediatR;
using Thing.Core.Contracts;

namespace Thing.Core.Command
{
    public class ThingCommandHandlers : IAsyncRequestHandler<ThingCommand.Create, Unit>,
        IAsyncRequestHandler<ThingCommand.Delete, Unit>
    {
        private readonly IThingRepository _thingRepository;

        public ThingCommandHandlers(IThingRepository thingRepository)
        {
            _thingRepository = thingRepository;
        }

        public async Task<Unit> Handle(ThingCommand.Create message)
        {
#pragma warning disable 618
            var entity = new Domain.Thing(message.Id, message.Name).SetAddress(message.AddressLine, message.AddressZip);
#pragma warning restore 618

            await _thingRepository.Add(entity);

            return Unit.Value;
        }

        public async Task<Unit> Handle(ThingCommand.Delete message)
        {
            await _thingRepository.Delete(message.Id);

            return Unit.Value;
        }
    }
}