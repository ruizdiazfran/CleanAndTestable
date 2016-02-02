using FluentValidation;
using MediatR;

namespace SampleLibrary.Command
{
    public class Thing
    {
        public class Create : IAsyncRequest<Unit>
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class CreateValidator : AbstractValidator<Create>
        {
            public CreateValidator()
            {
                RuleFor(_ => _.Id).NotEmpty();
                RuleFor(_ => _.Name).NotEmpty();
            }
        }

        public class Update : IAsyncRequest<Unit>
        {
            public string Name { get; set; }
        }

        public class UpdateValidator : AbstractValidator<Update>
        {
            public UpdateValidator()
            {
                RuleFor(_ => _.Name).NotEmpty();
            }
        }
    }
}