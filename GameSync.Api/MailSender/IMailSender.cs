namespace GameSync.Api.MailSender
{
    public interface IMailSender
    {
        Task<bool> SendMailAsync(string subject, string htmlBody, string recipient);
    }
}
