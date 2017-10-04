using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpKokoDemo.Requests
{
    public class GetProductRequest
    {
        [Required]
        public int MerchantId { get; set; }
    }
}
