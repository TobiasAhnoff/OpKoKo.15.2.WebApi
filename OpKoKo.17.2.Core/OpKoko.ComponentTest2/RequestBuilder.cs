using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using OpKokoDemo.Requests;

namespace OpKokoDemo.ComponentTest
{
    public static class RequestBuilder
    {
        public static HttpContent BuildJsonHttpContent<T>(T contentObject) where T : Request
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(contentObject));
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            return httpContent;
        }

        public static HttpRequestMessage BuildAuthorizedJsonHttpPostRequest<T>(T contentObject, string requestPath) where T : Request
        {
            var content = JsonConvert.SerializeObject(contentObject);

            var request = new HttpRequestMessage(HttpMethod.Post, requestPath)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json"),
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("SharedKey", CreateAuthorizationHeaderValue(requestPath, content));

            return request;
        }

        public static HttpRequestMessage BuildAuthorizedJsonHttpPutRequest<T>(T contentObject, string requestPath) where T : Request
        {
            var content = JsonConvert.SerializeObject(contentObject);

            var request = new HttpRequestMessage(HttpMethod.Put, requestPath)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json"),
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("SharedKey", CreateAuthorizationHeaderValue(requestPath, content));

            return request;
        }

        public static HttpRequestMessage BuildAuthorizedHttpGetRequest(string requestPath)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestPath);

            request.Headers.Authorization = new AuthenticationHeaderValue("SharedKey", CreateAuthorizationHeaderValue(requestPath, string.Empty));

            return request;
        }

        private static string CreateAuthorizationHeaderValue(string path, string content)
        {
            var authorizationHash = ComputeSHA256Hash($"{content}{path}apisharedkey");
            var authorizationPair = $"test:{authorizationHash}";

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(authorizationPair));
        }

        private static string ComputeSHA256Hash(string input)
        {
            using (var crypt = SHA256.Create())
            {
                var hash = string.Empty;
                var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
                return crypto.Aggregate(hash, (current, theByte) => current + theByte.ToString("x2"));
            }
        }
    }

}
