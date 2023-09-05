namespace GameSync.Api.AuthMailServices;

public interface IConfirmationEmailSender
{
    Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken);
}