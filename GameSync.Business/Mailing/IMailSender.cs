namespace GameSync.Business.Mailing
{
    public interface IMailSender
    {
        Task<bool> SendMailAsync(string subject, string htmlBody, string recipient);
    }
}
