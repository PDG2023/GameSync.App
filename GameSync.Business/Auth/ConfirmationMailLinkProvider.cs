using Microsoft.AspNetCore.Http;

namespace GameSync.Business.Auth;

public class ConfirmationMailLinkProvider
{

    public string GetConfirmationMailLink(string toEmail, string mailConfirmationToken, string scheme, string host)
    {

        return $"{scheme}://{host}/api/users/confirm?confirmationToken={mailConfirmationToken}&email={toEmail}";
    }
}