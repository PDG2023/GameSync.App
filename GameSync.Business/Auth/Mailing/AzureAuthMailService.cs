using Azure;
using Azure.Communication.Email;

namespace GameSync.Business.Auth.Mailing
{
    public class AzureAuthMailService : IAuthMailService
    {

        public async Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken)
        {
            // This code demonstrates how to send email using Azure Communication Services.
            var emailClient = new EmailClient(connectionString);
            var message = BuildEmail(sender, toEmail, mailConfirmationToken);

            try
            {

                var emailSendOperation = await emailClient.SendAsync(Azure.WaitUntil.Completed, message);
                var sendResult = emailSendOperation.Value;
                return true;
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"Email send operation failed with error code: {e.ErrorCode}, message: {e.Message}");
                return false;
            }

        }

        private static EmailMessage BuildEmail(string senderMail, string recipientMail, string confirmationToken)
        {
            var link = new ConfirmationMailLinkProvider().GetConfirmationMailLink(recipientMail, confirmationToken, "http", "localhost");
            var content = new EmailContent("Confirm your registration")
            {
                Html =  $"<a href=\"{link}\">Confirm your mail</a>"
            };
            return new EmailMessage(senderMail, recipientMail, content);
        }
    }
}

           