using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using OpKokoDemo.ComponentTest;
using OpKokoDemo.Exceptions;
using OpKokoDemo.Models;
using OpKokoDemo.Requests;
using OpKokoDemo.Responses;
using Ploeh.AutoFixture;
using Assert = NUnit.Framework.Assert;

namespace OpKoko.ComponentTest2
{
    public class ControllerTest
    {
        public class WhenAddingANewProduct : ComponentTest
        {
            private AddProductRequest _request;

            private HttpResponseMessage _response;
            private int _merchantId;
            private AddProductResponse _result;

            protected override void Setup()
            {
                _merchantId = 5;
                _request = new AddProductRequest
                {
                    Description = "Description",
                    Language = Language.SE.ToString(),
                    Name = "Name",
                    Price = 100
                };
            }

            protected override void Act()
            {
                _response = Client.PostAsync($"products/{_merchantId}", RequestBuilder.BuildJsonHttpContent(_request))
                    .Result;
            }

            protected override void AssertionPreparation()
            {
                var content = _response.Content.ReadAsStringAsync().Result;
                _result = JsonConvert.DeserializeObject<AddProductResponse>(content);
            }

            [Test]
            public void Then_the_response_returns_http_status_ok() =>
                Assert.AreEqual(HttpStatusCode.OK, _response.StatusCode);

            [Test]
            public void Then_name_is_mapped() => Assert.AreEqual(_request.Name, _result.Product.Name);

            [Test]
            public void Then_price_is_mapped() => Assert.AreEqual(_request.Price, _result.Product.Price);

            [Test]
            public void Then_language_is_mapped() =>
                Assert.AreEqual(_request.Language, _result.Product.Language.ToString());

            [Test]
            public void Then_merchant_id_is_mapped() => Assert.AreEqual(_merchantId, _result.Product.MerchantId);
        }

        public class WhenAddingANewProductToANonExistingMerchant : ComponentTest
        {
            private AddProductRequest _request;

            private int _merchantId;
            private HttpResponseMessage _response;
            private int _expectedProductCount;
            private int _newProductCount;
            private Language _language = Language.SE;

            protected override void Setup()
            {
                _merchantId = int.MaxValue;
                _request = Fixture.Build<AddProductRequest>()
                    .With(p => p.Price, 1000)
                    .With(p => p.Language, _language.ToString())
                    .Create();
            }

            protected override void Act()
            {
                _expectedProductCount = Repository.GetProducts(-1).Result.Count();
                _response = Client.PostAsync($"products/{_merchantId}", RequestBuilder.BuildJsonHttpContent(_request))
                    .Result;
            }

            protected override void AssertionPreparation()
            {
                _newProductCount = Repository.GetProducts(-1).Result.Count();
            }

            [Test]
            public void Then_the_response_is_bad_request() => Assert.AreEqual(HttpStatusCode.BadRequest, _response.StatusCode);

            [Test]
            public void Then_no_product_is_added() => Assert.AreEqual(_expectedProductCount, _newProductCount);

        }

        public class WhenAddingANewProductWithInvalidCountryCode : ComponentTest
        {
            private AddProductRequest _request;

            private int _merchantId;
            private HttpResponseMessage _response;
            private List<Error> _errors;

            protected override void Setup()
            {
                _merchantId = 10;
                _request = Fixture.Build<AddProductRequest>()
                    .With(p => p.Price, 1000)
                    .Create();
            }

            protected override void Act()
            {
                _response = Client.PostAsync($"products/{_merchantId}", RequestBuilder.BuildJsonHttpContent(_request))
                    .Result;
            }

            protected override void AssertionPreparation()
            {
                var response = _response.Content.ReadAsStringAsync().Result;
                _errors = JsonConvert.DeserializeObject<List<Error>>(response);
            }

            [Test]
            public void Then_the_response_is_bad_request() => Assert.AreEqual(HttpStatusCode.BadRequest, _response.StatusCode);

            [Test]
            public void Then_the_error_response_contains_language_error() => Assert.IsTrue(_errors.Any(e => e.Message == $"{nameof(_request.Language)} is not a valid language."));

        }

        public class WhenDeletingAProduct : ComponentTest
        {
            private HttpResponseMessage _response;
            private int _merchantId;
            private int _productId;
            private int _expectedProductCount;
            private Language _language;
            private int _newProductCount;

            protected override void Setup()
            {
                _merchantId = 1;
                _productId = 1;
            }

            protected override void Act()
            {
                _language = Language.SE;
                _expectedProductCount = Repository.GetProducts(-1).Result.Count();
                _response = Client.DeleteAsync($"products/{_merchantId}/{_productId}")
                    .Result;
            }

            protected override void AssertionPreparation()
            {
                _newProductCount = Repository.GetProducts(-1).Result.Count();
            }

            [Test]
            public void Then_the_response_returns_http_status_ok() =>
                Assert.AreEqual(HttpStatusCode.OK, _response.StatusCode);

            [Test]
            public void Then_the_product_is_removed() => Assert.IsTrue(_newProductCount == (_expectedProductCount - 1));
        }

        public class WhenDeletingAProductThatDoesNotExist : ComponentTest
        {
            private HttpResponseMessage _response;
            private int _merchantId;
            private int _productId;
            private int _expectedProductCount;
            private Language _language;
            private int _newProductCount;

            protected override void Setup()
            {
                _merchantId = Fixture.Create<int>();
                _productId = Fixture.Create<int>();
            }

            protected override void Act()
            {
                _language = Language.SE;
                _expectedProductCount = Repository.GetProducts(-1).Result.Count();
                _response = Client.DeleteAsync($"products/{_merchantId}/{_productId}")
                    .Result;
            }

            protected override void AssertionPreparation()
            {
                _newProductCount = Repository.GetProducts(-1).Result.Count();
            }

            [Test]
            public void Then_the_response_is_bad_request() => Assert.AreEqual(HttpStatusCode.BadRequest, _response.StatusCode);

            [Test]
            public void Then_the_product_is_removed() => Assert.IsTrue(_newProductCount == _expectedProductCount);
        }

        public class When_attempting_to_get_products_with_unsupported_language : ComponentTest
        {
            private HttpResponseMessage _response;
            private int _merchantId;
            private int _productId;
            private int _expectedProductCount;
            private Language _language;
            private int _newProductCount;
            private GetProductRequest _request;
            private ExceptionResult _error;

            protected override void Setup()
            {
                _request = new GetProductRequest
                {
                    Language = Language.NO
                };
            }

            protected override void Act()
            {
                _response = Client.GetAsync(CreateUri(_merchantId, _request))
                    .Result;
            }

         

            [Test]
            public void Then_the_response_is_bad_request() => Assert.AreEqual(HttpStatusCode.BadRequest, _response.StatusCode);

            
        }

        public static string CreateUri(int merchantId, Request request)
        {
            var queryString = request.ToQueryString();

            return $"products/{merchantId}?{queryString}";
        }

        public static T GetResponseData<T>(IActionResult actionResult)
        {
            return (T)(actionResult as ObjectResult)?.Value;
        }
    }
}
