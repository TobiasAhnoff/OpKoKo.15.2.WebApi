using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OpKokoDemo.Exceptions;

namespace OpKokoDemo.Results
{
    public class ModelStateErrorObjectResult : ObjectResult
    {
        public List<Error> Errors => Value as List<Error>;

        public ModelStateErrorObjectResult(List<Error> errorMessages)
            : base(errorMessages) => StatusCode = 400;
    }
}
