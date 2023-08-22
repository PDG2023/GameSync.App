using GameSync.Business.Auth;
using static Duende.IdentityServer.Models.IdentityResources;

namespace GameSync.Api.Tests.Identity;

public class MockMailService : IConfirmationEmailSender, IForgotPasswordEmailSender
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

    public Task<bool> SendForgotPasswordEmailAsync(string recipient, string passwordChangeToken)
    {
        return Register(recipient, passwordChangeToken);
    }
}
