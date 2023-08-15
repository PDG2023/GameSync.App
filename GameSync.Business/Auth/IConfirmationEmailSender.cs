namespace GameSync.Business.Auth;

public interface IConfirmationEmailSender
{
    Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken);
}