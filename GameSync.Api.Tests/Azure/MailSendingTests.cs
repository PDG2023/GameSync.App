using GameSync.Business.Auth.Mailing;
using Xunit;

namespace GameSync.Api.Tests.Azure
{
    [Collection("FullApp")]
    public class MailSendingTests 
    {
        [Fact]
        public async Task Sending_mail_with_azure_communication_service_should_work()
        {
            // arrange
            var service = new AzureAuthMailService();

            // act
            var mailSent = await service.SendEmailConfirmationAsync("ferati.kevin@gmail.com", "test");

            // assert
            Assert.True(mailSent);
        }
    }
}
