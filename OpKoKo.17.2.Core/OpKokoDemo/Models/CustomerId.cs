using System;

namespace OpKokoDemo.Models
{
    public class CustomerId
    {
        public CustomerId(int id)
        {
            if (id >= -1 && id < 100)
            {
                Id = id;
            }
            else
            {
                throw new ArgumentException("A customer id must be between -1 and 99", nameof(id));
            }
        }

        public int Id { get; }
    }
}
