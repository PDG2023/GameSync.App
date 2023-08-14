using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GameSync.Business.Auth;

public class ConfirmationMailLinkProvider
{

    public string GetConfirmationMailLink(string toEmail, string mailConfirmationToken, string scheme, string host)
    {
        string b64UrlToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(mailConfirmationToken));
        return $"{scheme}://{host}/api/users/confirm?confirmationToken={b64UrlToken}&email={toEmail}";
    }
}