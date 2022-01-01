using IntegrationTesting.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;

namespace Integrationtesting.Test
{
    public class UnitTest1
    {
        private readonly WebApplicationFactory<Startup> app;

        public UnitTest1()
        {
            app = new WebApplicationFactory<Startup>();
          // this.app = app;
           
        }
        [Fact]
        public async void CheckWeather_Service_is_Working()
        {
            var httpClient = app.CreateClient();
            var response = await httpClient.GetAsync("WeatherForecast");
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);
        }
    }
}
