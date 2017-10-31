using System;

namespace OpKokoDemo.Requests
{
    public abstract class Request
    {
        public Guid CorrelationId { get; set; }

        public string Context { get; set; }

        public bool AlwaysReturnHttpOkStatus { get; set; }
    }
}
