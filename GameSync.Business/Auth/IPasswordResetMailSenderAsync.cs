namespace GameSync.Business.Auth;

public interface IPasswordResetMailSenderAsync
{
    Task<bool> SendEmailPasswordResetAsync(string recipient, string passwordChangeToken);
}
