using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpKokoDemo.Validation
{
    public class PropertyValidationError
    {
        public string PropertyName { get; }

        public string ErrorMessage { get; }

        public PropertyValidationError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }
}
