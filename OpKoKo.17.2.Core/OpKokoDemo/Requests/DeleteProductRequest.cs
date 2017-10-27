using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpKokoDemo.Requests
{
    public class DeleteProductRequest
    {
        [Required]
        public int ProductId { get; set; }
    }
}
