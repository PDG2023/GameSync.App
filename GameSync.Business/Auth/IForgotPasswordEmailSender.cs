namespace GameSync.Business.Auth;

public interface IForgotPasswordEmailSender
{
    Task<bool> SendForgotPasswordEmailAsync(string recipient, string passwordChangeToken);
}
