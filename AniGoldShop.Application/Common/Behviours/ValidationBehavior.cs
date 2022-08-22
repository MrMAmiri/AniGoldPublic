using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace AniGoldShop.Application.Common.Behviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new FluentValidation.ValidationContext<TRequest>(request);
            var failurs = _validators
                .Select(s => s.Validate(context))
                .SelectMany(s => s.Errors)
                .Where(s => s != null)
                .ToList();

            if (failurs.Any())
            {
                string errorMessage = "";
                foreach (var error in failurs)
                {
                    errorMessage += $"{error.ErrorMessage}\r\n";
                }

                //throw new ValidationException(errorMessage);
            }

            return next();
        }
    }
}
