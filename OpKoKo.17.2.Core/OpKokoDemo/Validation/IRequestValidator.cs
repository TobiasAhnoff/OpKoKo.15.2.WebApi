using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpKokoDemo.Models;

namespace OpKokoDemo.Validation
{
    public interface IRequestValidator<in T> : IRequestValidator
        where T : IValidatableRequest
    {
        void Validate(T request);
    }

    public interface IRequestValidator
    {
        IEnumerable<PropertyValidationError> Validate(IValidatableRequest request);

        Type RequestType { get; }

        Language Language { get; }

        bool ValidFor(Language language);
    }
}
