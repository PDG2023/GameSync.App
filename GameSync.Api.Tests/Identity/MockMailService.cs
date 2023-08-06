using GameSync.Business.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSync.Api.Tests.Identity;

public class MockMailService : IAuthMailService
{
    public Dictionary<string, string> Mails { get; private set; } = new Dictionary<string, string>();

    public Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken)
    {
        Mails[toEmail] = mailConfirmationToken;
        return Task.FromResult(true);
    }
}
