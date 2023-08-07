using GameSync.Business.Auth.Mailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSync.Api.Tests.Identity;

public class MockMailService : IAuthMailService
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
}
