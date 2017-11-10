namespace OpKokoDemo.Models
{
    public class Product
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                var idModel = new ProductId(value);
                _id = idModel.Id;
            }
        }

        private int _customerId;
        public int CustomerId
        {
            get => _customerId;
            set
            {
                var idModel = new CustomerId(value);
                _customerId = idModel.Id;
            }
        }

        public string Name { get; }

        public int Price { get; }

        public Language Language { get; }

        public Product(int customerId, string name, int price, Language language)
        {
            // Do more complex validation here

            CustomerId = customerId;
            Name = name;
            Price = price;
            Language = language;
        }
    }
}
