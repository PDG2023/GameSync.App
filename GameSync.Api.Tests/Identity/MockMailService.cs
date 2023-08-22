using GameSync.Business.Auth;

namespace GameSync.Api.Tests.Identity;

public class MockMailService : IConfirmationEmailSender, IForgotPasswordEmailSender
{
    private readonly bool _shouldFail;

    public MockMailService(bool shouldFail)
    {
        _shouldFail = shouldFail;
    }

    public Dictionary<string, string> Mails { get; private set; } = new Dictionary<string, string>();

    public Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken)
    {
        if (!_shouldFail)
        {
            Mails[toEmail] = mailConfirmationToken;
        }
        
        return Task.FromResult(!_shouldFail);
    }

    public Task<bool> SendForgotPasswordEmailAsync(string recipient, string passwordChangeToken)
    {
        throw new NotImplementedException();
    }
}
