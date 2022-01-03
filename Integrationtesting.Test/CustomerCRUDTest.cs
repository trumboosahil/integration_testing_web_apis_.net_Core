using IntegrationTesting.Web;
using IntegrationTesting.Web.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Integrationtesting.Test
{
    [Collection("Sequential")]
    public  class CustomerCRUDTest:IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public CustomerCRUDTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;

        }

        [Theory]
        [InlineData(1)]

        public async Task Check_NotFoundWhenNoCustomerExists(int id)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/Customer/{id}").ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
       
        [Fact]
        public async Task AddCustomer()
        {
            var client = _factory.CreateClient();
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var customer =new Customer
            {
                ID = 1,
                FirstName = "Shafi",
                LastName = "Trumboo"
            };

            string jsn = JsonConvert.SerializeObject(customer, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            var content = new StringContent(jsn, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("/Customer", content);


            httpResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            var responsecontent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var customerresponse = JsonConvert.DeserializeObject<Customer>(responsecontent);
            Assert.NotNull(customerresponse);
            Assert.Equal(customerresponse.FirstName, customer.FirstName);

        }
        [Theory]
        [InlineData(1)]
        public async Task Check_CustomerExistWhenOneIsInserted(int id)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/Customer/{id}").ConfigureAwait(false);          
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var customer = JsonConvert.DeserializeObject<Customer>(content);
            Assert.NotNull(customer);
            Assert.Equal(id, customer.ID);

        }
    }
}
