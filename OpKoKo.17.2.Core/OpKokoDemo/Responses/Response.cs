using System;
using Newtonsoft.Json;
using OpKokoDemo.Exceptions;

namespace OpKokoDemo.Responses
{
    public class Response
    {
        [JsonProperty(PropertyName = "id")]
        public Guid CorrelationId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Context { get; set; }

        public object Data { get; set; }

        public ErrorDetails Error { get; set; }
    }
}
