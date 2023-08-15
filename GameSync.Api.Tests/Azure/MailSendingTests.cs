using Castle.Core.Logging;
using FakeItEasy;
using GameSync.Business.Mailing;
using Microsoft.Extensions.Logging;
using Xunit;

namespace GameSync.Api.Tests.Azure
{
    public class MailSendingTests 
    {
        [Fact]
        public async Task Sending_mail_with_azure_communication_service_should_work()
        {
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
