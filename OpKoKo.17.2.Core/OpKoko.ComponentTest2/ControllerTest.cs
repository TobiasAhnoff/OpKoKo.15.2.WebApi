using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using OpKokoDemo.Requests;
using OpKokoDemo.Responses;
using Ploeh.AutoFixture;
using Assert = NUnit.Framework.Assert;

namespace OpKokoDemo.ComponentTest
{
    public class ControllerTest
    {
        [Test]
        public void Test()
        {
            Assert.IsTrue(1 == 1);
        }
        public class When_ : ComponentTest
        {
            private AddProductRequest _request;
            public When_()
            {
            }

            private HttpResponseMessage _response;

            protected override void Setup()
            {
                _request = Fixture.Build<AddProductRequest>()
                    .With(p => p.MerchantId, 1)
                    .With(p => p.Price, 123).Create();
            }

            protected override void Act()
            {
                _response = Client.PostAsync($"products/123", RequestBuilder.BuildJsonHttpContent(_request)).Result;
            }

            protected override void AssertionPreparation()
            {
                ////_productAdded = (_response. as OkObjectResult).Value as AddProductResponse;
            }

            [Test]
            public void Then_the_response_returns_http_status_ok() => Assert.AreEqual(HttpStatusCode.OK, _response.StatusCode);

            ////[Test]
            ////public void Then_name_is_mapped() => Assert.AreEqual(_request, _response.);
        }
    }
}
