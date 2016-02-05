using System;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Should;
using Thing.Core.Command;

namespace Thing.Tests.Integration.Db
{
    public class ContainerTests
    {
        private readonly IMediator _mediator;

        public ContainerTests(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Should_not_create_things()
        {
            try
            {
                await _mediator.SendAsync(new ThingCommand.Create());
            }
            catch (Exception ex)
            {
                ex.ShouldBeType(typeof (ValidationException));
            }
        }

        public async Task Should_create_things()
        {
            var request = new ThingCommand.Create {Id = "0", Name = "Test", AddressLine = "Test", AddressZip = "20133"};
            await _mediator.SendAsync(request);
        }
    }
}