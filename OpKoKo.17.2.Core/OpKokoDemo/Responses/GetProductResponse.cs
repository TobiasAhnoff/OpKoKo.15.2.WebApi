using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpKokoDemo.Models;

namespace OpKokoDemo.Responses
{
    public class GetProductResponse
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
