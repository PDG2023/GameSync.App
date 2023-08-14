namespace GameSync.Business.Auth.Mailing;

public interface IAuthMailService
{
    Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken);
}