using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using MediatR;
using Should;
using Should.Core.Exceptions;
using Thing.Core.Command;
using Thing.Core.Contracts;
using Thing.Core.Query;

namespace Thing.Tests.Integration.Db.Specs
{
    public class ThingSpecs : SpecsForDb
    {
        private readonly IMediator _mediator;

        public ThingSpecs(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Should_get_all(ThingQuery.GetAll request)
        {
            //  Arrange

            //  Act
            var result = _mediator.SendAsync(request).Result;

            //  Assert
            result.Count.ShouldEqual(4);
        }

        public void Should_get_one(ThingQuery.GetById request)
        {
            //  Arrange
            request.Id = "my-first";

            //  Act
            var result = _mediator.SendAsync(request).Result;

            //  Assert
            result.Id.ShouldEqual(request.Id);
        }

        public void Should_throw_ex_when_is_not_exists(ThingQuery.GetById request)
        {
            try
            {
                //  Arrange

                //  Act
                _mediator.SendAsync(request).Wait();
            }
            catch (AggregateException ex)
            {
                //  Assert
                ex.InnerException.ShouldBeType(typeof (EntityNotFound));
                return;
            }

            throw new AssertException();
        }

        public void Should_not_create_when_id_is_null(ThingCommand.Create request)
        {
            try
            {
                //  Arrange
                request.Id = null;

                //  Act
                Persist(() => _mediator.SendAsync(request).Wait());
            }
            catch (Exception ex)
            {
                //  Assert
                ex.ShouldBeType(typeof (DbEntityValidationException));
                return;
            }

            throw new AssertException();
        }

        public void Should_create(ThingCommand.Create request)
        {
            //  Arrange

            //  Act
            Persist(() => _mediator.SendAsync(request).Wait());

            //  Assert
            Do(db => db.Things.AnyAsync(_ => _.Id == request.Id).Result.ShouldBeTrue());
        }

        public void Should_not_create_when_name_is_secret(ThingCommand.Create request)
        {
            //  Arrange
            request.Name = "secret";

            //  Act
            Persist(() => _mediator.SendAsync(request).Wait());

            //  Assert
            Do(db => db.Things.AnyAsync(_ => _.Id == request.Id).Result.ShouldBeFalse());
        }

        public void Should_delete(ThingCommand.Delete request)
        {
            //  Arrange
            request.Id = "my-thirdy";

            //  Act
            Persist(() => _mediator.SendAsync(request).Wait());

            //  Assert
            Do(db => db.Things.AnyAsync(_ => _.Id == request.Id).Result.ShouldBeFalse());
        }

        public void Should_not_delete_when_name_is_secret(ThingCommand.Delete request)
        {
            //  Arrange
            request.Id = "my-fourthy";

            //  Act
            Persist(() => _mediator.SendAsync(request).Wait());

            //  Assert
            Do(db => db.Things.AnyAsync(_ => _.Id == request.Id).Result.ShouldBeTrue());
        }
    }
}