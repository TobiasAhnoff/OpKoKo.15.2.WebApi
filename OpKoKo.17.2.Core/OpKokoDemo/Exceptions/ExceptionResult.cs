using Microsoft.AspNetCore.Mvc;

namespace OpKokoDemo.Exceptions
{
    public class ExceptionResult : ActionResult
    {
        public int HttpStatusCode { get; }

        public string HttpStatusMessage { get; }

        public string Message { get; }

        public string Reason { get; }

        public ExceptionResult()
        {
            HttpStatusCode = 500;
            HttpStatusMessage = HttpStatusCodeTexts.StatusCodes[HttpStatusCode];
            Reason = "UnhandledException";
            Message = "An unhandled exception occured. Please contact somebody!";
        }

        public ExceptionResult(string reason, string message, int httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
            HttpStatusMessage = HttpStatusCodeTexts.StatusCodes[HttpStatusCode];
            Reason = reason;
            Message = message;
        }
    }
}
