using FluentValidation;
using MediatR;

namespace SampleLibrary.Command
{
    public class ThingCommand
    {
        public class Create : IAsyncRequest<Unit>
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
    }
}