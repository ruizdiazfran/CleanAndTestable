using System;
using System.Threading.Tasks;
using MediatR;

namespace SampleLibrary.Command
{
    public class ThingCommandHandlers : IAsyncRequestHandler<Thing.Create, Unit>,
        IAsyncRequestHandler<Thing.Update, Unit>
    {
        Task<Unit> IAsyncRequestHandler<Thing.Create, Unit>.Handle(Thing.Create message)
        {
            throw new NotImplementedException();
        }

        Task<Unit> IAsyncRequestHandler<Thing.Update, Unit>.Handle(Thing.Update message)
        {
            throw new NotImplementedException();
        }
    }
}