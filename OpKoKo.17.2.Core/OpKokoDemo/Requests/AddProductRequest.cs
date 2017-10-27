using System.ComponentModel.DataAnnotations;
using OpKokoDemo.Attributes;
using OpKokoDemo.Validation;

namespace OpKokoDemo.Requests
{
    [AddProductValidator]
    public class AddProductRequest : Request
    {
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
