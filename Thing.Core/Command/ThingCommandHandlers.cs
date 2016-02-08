using System.Threading.Tasks;
using MediatR;
using Thing.Core.Contracts;

namespace Thing.Core.Command
{
    public class ThingCommandHandlers : IAsyncRequestHandler<ThingCommand.Create, Unit>,
        IAsyncRequestHandler<ThingCommand.Delete, Unit>
    {
        private readonly ISecurityPoint _securityPoint;
        private readonly IThingRepository _thingRepository;

        public ThingCommandHandlers(IThingRepository thingRepository, ISecurityPoint securityPoint)
        {
            _thingRepository = thingRepository;
            _securityPoint = securityPoint;
        }

        public async Task<Unit> Handle(ThingCommand.Create message)
        {
#pragma warning disable 618
            var entity = new Domain.Thing(message.Id, message.Name).SetAddress(message.AddressLine, message.AddressZip);
#pragma warning restore 618

            if (_securityPoint.CanDoWork(entity))
            {
                await _thingRepository.Add(entity);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(ThingCommand.Delete message)
        {
            var entity = await _thingRepository.GetByIdAsync(message.Id);

            if (_securityPoint.CanDoWork(entity))
            {
                await _thingRepository.Delete(message.Id);
            }

            return Unit.Value;
        }
    }
}