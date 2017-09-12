using System.ComponentModel.DataAnnotations;

namespace OpKokoDemo.Requests
{
    public class ExecuteServiceRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Uri { get; set; }

    }
}
