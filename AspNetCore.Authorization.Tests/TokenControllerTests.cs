using AspNetCore.Authorization.JsonWebToken;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace AspNetCore.Authorization.Tests
{
    public class TokenControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Token()
        {
            // Arrange
            var user = "user";
            var authInput = new AuthenticationInput(user, "pass");

            // Act
            var response = await _client.PostAsJsonAsync("/token", authInput);

            // Assert
            response.EnsureSuccessStatusCode();
            var jwt = await response.Content.ReadFromJsonAsync<Jwt>();
            Assert.Equal(user, jwt.User);
            Assert.NotNull(jwt.ExpiresAt);
            Assert.NotNull(jwt.JwToken);
        }
    }
}