using System;
using System.Data.Entity;
using FluentValidation;
using MediatR;
using Should;
using Thing.Core.Command;
using Thing.Core.Query;

namespace Thing.Tests.Integration.Db
{
    public class ThingTests : BaseTest
    {
        private readonly IMediator _mediator;

        public ThingTests(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Should_get_all_things()
        {
            var request = new ThingQuery.GetAll();
            var result = _mediator.SendAsync(request).Result;
            result.Count.ShouldEqual(4);
        }

        public void Should_get_one_things()
        {
            var request = new ThingQuery.GetById {Id = "my-first"};
            var result = _mediator.SendAsync(request).Result;
            result.Id.ShouldEqual("my-first");
        }

        public void Should_not_create_things()
        {
            var request = new ThingCommand.Create();
            try
            {
                _mediator.SendAsync(request).Wait();
            }
            catch (AggregateException ex)
            {
                ex.InnerException.ShouldBeType(typeof (ValidationException));
            }
        }

        public void Should_create_things()
        {
            const string id = "test-id";
            var request = new ThingCommand.Create {Id = id, Name = "Test", AddressLine = "Test", AddressZip = "20133"};
            Tx(db => _mediator.SendAsync(request).Wait());
            Check(db => db.Things.AnyAsync(_ => _.Id == id).Result.ShouldBeTrue());
        }

        public void Should_not_create_secret_things()
        {
            const string id = "test-secret-id";
            var request = new ThingCommand.Create { Id = id, Name = "secret", AddressLine = "Test", AddressZip = "20133" };
            Tx(db => _mediator.SendAsync(request).Wait());
            Check(db => db.Things.AnyAsync(_ => _.Id == id).Result.ShouldBeFalse());
        }

        public void Should_delete_things()
        {
            const string id = "my-thirdy";
            var request = new ThingCommand.Delete {Id = id};
            Check(db => db.Things.FirstOrDefaultAsync(_ => _.Id == id).Result.ShouldNotBeNull());
            Tx(db => _mediator.SendAsync(request).Wait());
            Check(db => db.Things.AnyAsync(_ => _.Id == id).Result.ShouldBeFalse());
        }

        public void Should_not_delete_secret_things()
        {
            const string id = "my-fourthy";
            var request = new ThingCommand.Delete { Id = id };
            Check(db => db.Things.FirstOrDefaultAsync(_ => _.Id == id).Result.ShouldNotBeNull());
            Tx(db => _mediator.SendAsync(request).Wait());
            Check(db => db.Things.AnyAsync(_ => _.Id == id).Result.ShouldBeTrue());
        }
    }
}