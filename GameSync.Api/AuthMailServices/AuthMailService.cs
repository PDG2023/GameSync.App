using GameSync.Api.MailSender;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace GameSync.Api.AuthMailServices
{
    public class AuthMailService : IConfirmationEmailSender, IPasswordResetMailSender
    {
        private readonly IMailSender _sender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthMailService(
            IMailSender sender,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor)
        {
            _sender = sender;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> SendEmailConfirmationAsync(string recipient, string mailConfirmationToken)
        {
            var req = _contextAccessor.HttpContext.Request;
            var path = _configuration["FrontPathToMailConfirmatiom"];

            string b64UrlToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(mailConfirmationToken));
            var link =  $"{req.Scheme}://{req.Host}/{path}?confirmationToken={b64UrlToken}&email={recipient}";

            return await _sender.SendMailAsync(
                "GameSync - Confirmez votre nouveau compte",
                $"<a href=\"{link}\">Cliquez ici pour confirmer votre compte</a>",
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
