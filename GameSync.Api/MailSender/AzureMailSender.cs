using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Logging;

namespace GameSync.Api.MailSender;

public class AzureMailSender : IMailSender
{
    private readonly ILogger<AzureMailSender> _logger;

    public AzureMailSender(ILogger<AzureMailSender> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendMailAsync(string subject, string htmlBody, string recipient)
    {
        var connectionString = Environment.GetEnvironmentVariable("AZURE_MAIL_CONNECTION_STRING");
        var sender = Environment.GetEnvironmentVariable("AZURE_MAIL_SENDER");

        var emailClient = new EmailClient(connectionString);
        var content = new EmailContent(subject)
        {
            Html = htmlBody
        };

        var message = new EmailMessage(sender, recipient, content);

        try
        {
            var emailSendOperation = await emailClient.SendAsync(WaitUntil.Completed, message);
            var sendResult = emailSendOperation.Value;
            return true;
        }
        catch (RequestFailedException e)
        {
            _logger.LogError(e, "Error while sending a mail using azure");
            return false;
        }
    }
}

