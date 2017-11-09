namespace OpKokoDemo.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int CustomerId { get; }

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
