using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OpKokoDemo.Models;
using OpKokoDemo.Validation;

namespace OpKokoDemo.Requests
{
    public class GetProductRequest : Request, IValidatableRequest
    {
        [Required]
        public Language Language { get; set; }

        public string Pattern { get; set; }
    }
}
