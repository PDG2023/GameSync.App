using GameSync.Business.Mailing;
using Microsoft.AspNetCore.Http;

namespace GameSync.Business.Auth
{
    public class AuthMailService : IConfirmationEmailSender
    {
        private readonly IMailSender _sender;
        private readonly ConfirmationMailLinkProvider _emailLinkProvider;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthMailService(IMailSender sender, ConfirmationMailLinkProvider emailLinkProvider, IHttpContextAccessor contextAccessor)
        {
            this._sender = sender;
            _emailLinkProvider = emailLinkProvider;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> SendEmailConfirmationAsync(string recipient, string mailConfirmationToken)
        {
            var currentRequest = _contextAccessor.HttpContext.Request;
            var url = _emailLinkProvider.GetConfirmationMailLink(recipient, mailConfirmationToken, currentRequest.Scheme, currentRequest.Host.ToString());
            return await _sender.SendMailAsync("Confirm your new accout", $"<a href=\"{url}\">Confirmation link</a>", recipient);
        }
    }
}
