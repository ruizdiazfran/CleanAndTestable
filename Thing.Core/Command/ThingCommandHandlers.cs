using System.Threading.Tasks;
using MediatR;
using SampleLibrary.Contracts;
using SampleLibrary.Domain;

namespace SampleLibrary.Command
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
            var entity = new Thing(message.Id, message.Name).SetAddress(message.AddressLine, message.AddressZip);
#pragma warning restore 618

            _thingRepository.Add(entity);

            return Task.FromResult(Unit.Value);
        }
    }
}