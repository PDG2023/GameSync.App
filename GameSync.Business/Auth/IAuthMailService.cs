namespace GameSync.Business.Auth;

public interface IAuthMailService
{
    Task<bool> SendEmailConfirmation(string toEmail, string mailConfirmationToken);
}