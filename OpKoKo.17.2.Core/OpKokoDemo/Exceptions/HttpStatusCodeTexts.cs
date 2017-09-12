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
        public const string InternalServerError = "An unhandled exception has occured. Please contact Collector!";
        public const string ServiceUnavailable = "Service unavailable.";
        public const string BusinessError = "The request was rejected by the server due to a business error.";
        public static Dictionary<int, string> StatusCodes;

        static HttpStatusCodeTexts()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            int key1 = 400;
            string str1 = "Bad or faulty request. Please examine the errors property for details.";
            dictionary.Add(key1, str1);
            int key2 = 401;
            string str2 = "The resource requested requires authentication.";
            dictionary.Add(key2, str2);
            int key3 = 403;
            string str3 = "Access to the requested resource is denied.";
            dictionary.Add(key3, str3);
            int key4 = 404;
            string str4 = "The resource requested was not found.";
            dictionary.Add(key4, str4);
            int key5 = 409;
            string str5 = "The request could not be completed due to a conflict with the current state of the target resource.";
            dictionary.Add(key5, str5);
            int key6 = 423;
            string str6 = "The resource requested is currently locked for modification. Try again.";
            dictionary.Add(key6, str6);
            int key7 = 500;
            string str7 = "An unhandled exception has occured. Please contact Collector!";
            dictionary.Add(key7, str7);
            int key8 = 503;
            string str8 = "Service unavailable.";
            dictionary.Add(key8, str8);
            int key9 = 900;
            string str9 = "The request was rejected by the server due to a business error.";
            dictionary.Add(key9, str9);
            StatusCodes = dictionary;
        }
    }
}
