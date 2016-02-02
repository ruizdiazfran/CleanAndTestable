using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace SampleLibrary.Pipeline
{
    public class ValidatorHandlerPipelineAsync<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
        where TResponse : class
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _inner;
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorHandlerPipelineAsync(IAsyncRequestHandler<TRequest, TResponse> inner,
            IValidator<TRequest>[] validators)
        {
            _inner = inner;
            _validators = validators;
        }

        Task<TResponse> IAsyncRequestHandler<TRequest, TResponse>.Handle(TRequest message)
        {
            var context = new ValidationContext(message);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return _inner.Handle(message);
        }
    }
}