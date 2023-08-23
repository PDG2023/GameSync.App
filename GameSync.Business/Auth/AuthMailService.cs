using GameSync.Business.Mailing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GameSync.Business.Auth
{
    public class AuthMailService : IConfirmationEmailSender, IPasswordResetMailSenderAsync
    {
        private readonly IMailSender _sender;
        private readonly ConfirmationMailLinkProvider _emailLinkProvider;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthMailService(
            IMailSender sender, 
            ConfirmationMailLinkProvider emailLinkProvider,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor)
        {
            _sender = sender;
            _emailLinkProvider = emailLinkProvider;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> SendEmailConfirmationAsync(string recipient, string mailConfirmationToken)
        {
            var currentRequest = _contextAccessor.HttpContext.Request;
            var url = _emailLinkProvider.GetConfirmationMailLink(recipient, mailConfirmationToken, currentRequest.Scheme, currentRequest.Host.ToString());
            return await _sender.SendMailAsync(
                "GameSync - Confirmez votre nouveau compte", 
                $"<a href=\"{url}\">Cliquez ici pour confirmer votre compte</a>", 
                recipient);
        }

        public async Task<bool> SendEmailPasswordResetAsync(string recipient, string passwordChangeToken)
        {
            var currentRequest = _contextAccessor.HttpContext.Request;
            var forgotPasswordPath = _configuration["ForgotPasswordEmailPath"];
            var url = $"{currentRequest.Scheme}://{currentRequest.Host}/{forgotPasswordPath}?forgotPasswordToken={passwordChangeToken}&email={recipient}";

            return await _sender.SendMailAsync(
                "GameSync - Demande de changement de mot de passe", 
                $"<a href=\"{url}\">Cliquez ici pour changer votre mot de passe</a>",
                recipient);
        }
    }
}
