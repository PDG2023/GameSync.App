using GameSync.Business.Features.Search;
using System.Net.Http.Json;
using Xunit;

namespace GameSync.Api.Tests
{
    public class SignInTests : IClassFixture<GameSyncAppFactory>
    {
        private readonly GameSyncAppFactory _factory;

        public SignInTests(GameSyncAppFactory integrationTestFactory)
        {
            _factory = integrationTestFactory;
        }

        [Fact]
        public async Task SignInShouldCreateUserWithSecurePassword()
        {
            // arrange
            var client = _factory.CreateClient();
            var createUserPayload = """
                {
                    "username": "kevin",
                    "password": "GWyYw!w9i%LLQ*"
                }
                """;
            // act
            var response = await client.PostAsJsonAsync("/api/user/sign-in", createUserPayload);
            
            // assert
            response.EnsureSuccessStatusCode();
            // todo

        }
    }
}
