using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Thing.Core.Contracts;

namespace Thing.Core.Infrastructure.Pipeline
{
    public class AsyncValidationRequestHandler<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _inner;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public AsyncValidationRequestHandler(
            IAsyncRequestHandler<TRequest, TResponse> inner,
            IEnumerable<IValidator<TRequest>> validators)
        {
            Debug.WriteLine($"Create {GetType().Name}");
            _inner = inner;
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            if (!(message is IValidable))
            {
                return await _inner.Handle(message);
            }

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

            return await _inner.Handle(message);
        }
    }
}