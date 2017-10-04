using System.ComponentModel.DataAnnotations;
using OpKokoDemo.Attributes;

namespace OpKokoDemo.Requests
{
    [AddProductValidator]
    public class AddProductRequest : Request
    {
        [Required]
        public int MerchantId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Language { get; set; }

        public string Description { get; set; }
    }
}
