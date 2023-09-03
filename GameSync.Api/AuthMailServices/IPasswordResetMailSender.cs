namespace GameSync.Api.AuthMailServices;

public interface IPasswordResetMailSender
{
    Task<bool> SendEmailPasswordResetAsync(string recipient, string passwordChangeToken);
}
