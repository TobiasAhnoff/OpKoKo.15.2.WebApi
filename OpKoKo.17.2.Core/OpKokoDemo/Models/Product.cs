namespace OpKokoDemo.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int MerchantId { get; }

        public string Name { get; }

        public int Price { get; }

        public Language Language { get; }

        public Product(int merhantId, string name, int price, Language language)
        {
            // Do more complex validation here

            MerchantId = merhantId;
            Name = name;
            Price = price;
            Language = language;
        }
    }
}
