// Authentication/AuthenticationControllerTests.cs
using CleanArchitecture.Api.IntegrationTests.Common.WebApplicationFactory;

namespace CleanArchitecture.Api.IntegrationTests.Authentication;

[Collection(WebAppFactoryCollection.CollectionName)]
public class AuthenticationControllerTests(WebAppFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Register_ShouldReturnSuccess_WhenValid()
    {
        var request = new
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Password = "StrongPassword123",
        };

        var response = await _client.PostAsJsonAsync("/api/authentication/register", request);

        response.EnsureSuccessStatusCode();
    }
}
