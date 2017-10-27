using System.Collections.Generic;

namespace OpKokoDemo.Exceptions
{
    public static class HttpStatusCodeTexts
    {
        public const string BadRequest = "Bad or faulty request. Please examine the errors property for details.";
        public const string Unauthorized = "The resource requested requires authentication.";
        public const string Forbidden = "Access to the requested resource is denied.";
        public const string NotFound = "The resource requested was not found.";
        public const string Conflict = "The request could not be completed due to a conflict with the current state of the target resource.";
        public const string ResourceLocked = "The resource requested is currently locked for modification. Try again.";
        public const string InternalServerError = "An unhandled exception has occured. Please contact somebody!";
        public const string ServiceUnavailable = "Service unavailable.";
        public const string BusinessError = "The request was rejected by the server due to a business error.";
        public const string NotImplemented = "Not implemented.";

        public static Dictionary<int, string> StatusCodes { get; } = new Dictionary<int, string>
        {
            { 400, BadRequest },
            { 401, Unauthorized },
            { 403, Forbidden },
            { 404, NotFound },
            { 409, Conflict },
            { 423, ResourceLocked },
            { 500, InternalServerError },
            { 501, NotImplemented },
            { 503, ServiceUnavailable },
            { 900, BusinessError }
        };
    }
}
