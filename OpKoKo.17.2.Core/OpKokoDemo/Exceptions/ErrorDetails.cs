using System.Collections.Generic;

namespace OpKokoDemo.Exceptions
{
    public class ErrorDetails
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public List<Error> Errors { get; }

        public ErrorDetails()
        {
            Errors = new List<Error>();
        }
    }
}
