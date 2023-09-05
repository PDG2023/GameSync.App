using GameSync.Api.AuthMailServices;

namespace Tests.Mocks;

public class MockMailService : IConfirmationEmailSender, IPasswordResetMailSender
{
    private readonly bool _shouldFail;

    public MockMailService(bool shouldFail)
    {
        _shouldFail = shouldFail;
    }

    public Dictionary<string, string> Mails { get; private set; } = new Dictionary<string, string>();

    private Task<bool> Register(string email, string content)
    {
        if (!_shouldFail)
        {
            Mails[email] = content;
        }

        return Task.FromResult(!_shouldFail);
    }

    public Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken)
    {
        return Register(toEmail, mailConfirmationToken);
    }

    public Task<bool> SendEmailPasswordResetAsync(string recipient, string passwordChangeToken)
    {
        return Register(recipient, passwordChangeToken);
    }
}
