using GameSync.Api.Endpoints.Users;
using GameSync.Business.Features.Search;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests
{
    public class SignInTests : IClassFixture<GameSyncAppFactory>
    {
        private readonly GameSyncAppFactory _factory;
        private readonly ITestOutputHelper _output;

        public SignInTests(GameSyncAppFactory integrationTestFactory, ITestOutputHelper output)
        {
            _factory = integrationTestFactory;
            _output = output;
        }

        [Fact]
        public async Task SignInWithSecurePasswordShouldNotProduceError()
        {
            // arrange
            var client = _factory.CreateClient();
            var mail = "ferati.kevin@gmail.com";
            var createUserRequest = new SignInRequest { Email = mail, Password = "$UX#%A!qaphEL2" };
            // act
            var response = await client.PostAsJsonAsync("/api/users/sign-in", createUserRequest);
            
            // assert
            if (!response.IsSuccessStatusCode) 
            {
                _output.WriteLine(response.Content.ToString());
            }

            var payload = await response.Content.ReadFromJsonAsync<SignInValidResponse>();
            Assert.NotNull(payload);
            Assert.Equal(mail, payload.Email);

        }
    }
}
