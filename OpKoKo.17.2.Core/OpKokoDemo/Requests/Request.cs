using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpKokoDemo.Requests
{
    public abstract class Request
    {
        public Guid CorrelationId { get; set; }

        public string Context { get; set; }

        public bool AlwaysReturnHttpOkStatus { get; set; }
    }
}
