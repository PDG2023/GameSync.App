namespace GameSync.Business.Auth;

public interface IAuthMailService
{
    Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken);
}