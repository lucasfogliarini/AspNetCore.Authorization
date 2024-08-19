using AspNetCore.Authorization.JsonWebToken;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AspNetCore.Authorization.Tests
{
    public class WeatherForecastControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Get5WeatherForecastsAsync_ShouldReturnOk()
        {
            // Arrange
            await ConfigureAuthorizationAsync();

            // Act
            var forecast5Response = await _client.GetAsync("/WeatherForecast/5");

            // Assert
            Assert.Equal(HttpStatusCode.OK, forecast5Response.StatusCode);
        }

        [Fact]
        public async Task Get10WeatherForecastsAsync_ShouldReturnForbidden()
        {
            // Arrange
            await ConfigureAuthorizationAsync();

            // Act
            var forecast10Response = await _client.GetAsync("/WeatherForecast/10");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, forecast10Response.StatusCode);
        }

        private async Task ConfigureAuthorizationAsync()
        {
            var authInput = new AuthenticationInput("user", "pass");
            var response = await _client.PostAsJsonAsync("/token", authInput);
            response.EnsureSuccessStatusCode();
            var jwt = await response.Content.ReadFromJsonAsync<Jwt>();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.JwToken);
        }
    }
}