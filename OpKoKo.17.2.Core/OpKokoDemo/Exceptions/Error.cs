namespace OpKokoDemo.Exceptions
{
    public class Error
    {
        public string Reason { get; set; }

        public string Message { get; set; }

        public string Property { get; set; }

        public Error()
        {
        }

        public Error(string message)
        {
            Message = message;
        }
    }
}
