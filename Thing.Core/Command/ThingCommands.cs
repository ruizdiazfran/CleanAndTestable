using FluentValidation;
using MediatR;
using Thing.Core.Contracts;

namespace Thing.Core.Command
{
    public class ThingCommand
    {
        public class Create : IAsyncRequest<Unit>, IValidable
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string AddressLine { get; set; }
            public string AddressZip { get; set; }

            public class Validator : AbstractValidator<Create>
            {
                public Validator()
                {
                    RuleFor(_ => _.Name).NotEmpty();
                    RuleFor(_ => _.AddressLine).NotEmpty();
                    RuleFor(_ => _.AddressZip).NotEmpty();
                }
            }
        }

        public class Delete : IAsyncRequest<Unit>, IValidable
        {
            public string Id { get; set; }

            public class Validator : AbstractValidator<Delete>
            {
                public Validator()
                {
                    RuleFor(_ => _.Id).NotEmpty();
                }
            }
        }
    }
}