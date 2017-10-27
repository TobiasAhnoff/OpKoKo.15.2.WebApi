using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpKokoDemo.Models;

namespace OpKokoDemo.Validation
{
    public abstract class RequestValidator<T> : IRequestValidator<T>
        where T : IValidatableRequest
    {
        public Type RequestType => typeof(T);

        public abstract Language Language { get; }

        public abstract bool ValidFor(Language language);

        private readonly List<PropertyValidationError> _errors = new List<PropertyValidationError>();

        public IEnumerable<PropertyValidationError> Validate(IValidatableRequest request)
        {
            Validate((T)request);
            return _errors;
        }

        protected void AddError(string propertyName, string errorMessage)
        {
            _errors.Add(new PropertyValidationError(propertyName, errorMessage));
        }

        public abstract void Validate(T request);
    }
}
