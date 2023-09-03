using FakeItEasy;
using GameSync.Api.MailSender;
using Microsoft.Extensions.Logging;
using Xunit;

namespace GameSync.Api.Tests.Azure
{
    public class MailSendingTests 
    {
        [Fact]
        public async Task Sending_mail_with_azure_communication_service_should_work()
        {
            // Disable the test when the Env variable have not been set, i.e. in local dev, because xunit doesn't allow passing them and we dont
            // want the azure connection string being open in plain text here

            if (Environment.GetEnvironmentVariable("AZURE_MAIL_CONNECTION_STRING") is null)
            {
                return;
            }


            // arrange
            var mockLogger = A.Fake<Logger<AzureMailSender>>();
            var service = new AzureMailSender(mockLogger);

            // act
            var mailSent = await service.SendMailAsync("unit tests", "<b>test content</b>", "cigox97887@vreaa.com");

            // assert
            Assert.True(mailSent);
        }
    }
}
