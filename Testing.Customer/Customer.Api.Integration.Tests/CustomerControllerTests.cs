using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Customer.Contract;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Customer.Api.Integration.Tests
{
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public CustomerControllerTests(WebApplicationFactory<Startup> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task Post_WhenCalled_ReturnEmptyResponseWith200()
        {
            var expectedResult = string.Empty;
            var expectedStatusCode = HttpStatusCode.OK;

            var customer = new CustomerDto
            {
                FullName = "Celalettin Altıntaş",
                CityCode = "Btmn",
                BirthDate = new DateTime(1988, 05, 07)
            };

            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/customer", content);

            var actualResult = await response.Content.ReadAsStringAsync();
            var actualStatusCode = response.StatusCode;

            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnInsertedCustomer()
        {
            var expectedResult = string.Empty;
            var expectedStatusCode = HttpStatusCode.OK;

            var customer = new CustomerDto
            {
                FullName = "Celalettin Altıntaş",
                CityCode = "Btmn",
                BirthDate = new DateTime(1988, 05, 07)
            };

            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

            var postResponse = await _client.PostAsync("/api/customer", content);

            var postActualResult = await postResponse.Content.ReadAsStringAsync();
            var postActualStatusCode = postResponse.StatusCode;

            Assert.Equal(expectedResult, postActualResult);
            Assert.Equal(expectedStatusCode, postActualStatusCode);

            var getResponse = await _client.GetAsync("/api/customer");
            var getActualResult = await getResponse.Content.ReadAsStringAsync();
            var getResult = JsonConvert.DeserializeObject<List<CustomerDto>>(getActualResult).First();

            Assert.Equal(customer.FullName, getResult.FullName);
            Assert.Equal(customer.BirthDate, getResult.BirthDate);
            Assert.Equal(customer.CityCode, getResult.CityCode);
        }
    }
}