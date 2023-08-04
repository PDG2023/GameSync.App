using Bogus.DataSets;
using GameSync.Api.Endpoints.Users;
using GameSync.Business.Features.Search;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests
{
    public class SIgnUpTests : IClassFixture<GameSyncAppFactory>
    {
        private readonly GameSyncAppFactory _factory;
        private readonly ITestOutputHelper _output;

        public SIgnUpTests(GameSyncAppFactory integrationTestFactory, ITestOutputHelper output)
        {
            _factory = integrationTestFactory;
            _output = output;
        }

        [Fact]
        public async Task SecurePassword_ProduceNoError()
        {
            // arrange
            var testRequest = GetTestRequest("$UX#%A!qaphEL2");

            // act
            var response = await TryCreateAccout(testRequest);
            
            // assert
            var payload = await response.Content.ReadFromJsonAsync<SignUpValidResponse>();
            Assert.NotNull(payload);
            Assert.Equal(testRequest.Email, payload.Email);

        }
 

        [Theory]
        [InlineData("asfWJIj1421", "PasswordRequiresNonAlphanumeric")]
        [InlineData("$1241245ds125", "PasswordRequiresUpper")]
        [InlineData("$XaBaBweqgRw", "PasswordRequiresDigit")]
        [InlineData("$X3%A!q", "PasswordTooShort")]
        public async Task BadlyFormedPassword(string password, string expectedError)
        {
            // arrange
            var testRequest = GetTestRequest(password);

            // act
            var response = await TryCreateAccout(testRequest);

            // assert
            await AssertProduceError(expectedError, response);
        }

        [Fact]
        public async Task BadlyFormedMail()
        {
            var response = await TryCreateAccout(new SignUpRequest { Email = "ab", Password = "$UX#%A!qaphEL2a23" });

            await AssertProduceError("InvalidEmail", response);
        }


        private static SignUpRequest GetTestRequest(string password) => new SignUpRequest
        {
            Email = new Internet().Email(),
            Password = password
        };

        private async Task<HttpResponseMessage> TryCreateAccout(SignUpRequest req)
        {
            var client = _factory.CreateClient();
            return await client.PostAsJsonAsync("/api/users/sign-in", req);
        }

        private async Task AssertProduceError(string errorCode, HttpResponseMessage response)
        {
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var typedResponse = await response.Content.ReadFromJsonAsync<IEnumerable<IdentityError>>();
            Assert.NotNull(typedResponse);
            var error = Assert.Single(typedResponse);
            Assert.Equal(errorCode, error.Code);
        }


    }


}
