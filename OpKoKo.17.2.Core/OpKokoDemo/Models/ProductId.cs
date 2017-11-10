using System;

namespace OpKokoDemo.Models
{
    public class ProductId
    {
        public ProductId(int id)
        {
            if (id >= 0 && id < 300)
            {
                Id = id;
            }
            else
            {
                throw new ArgumentException("A product id must be between 0 and 299", nameof(id));
            }
        }

        public int Id { get; }
    }
}
