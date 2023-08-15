using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace GameSync.Business.Auth.Mailing
{
    public class SmtpAuthMailService : IAuthMailService
    {
        private readonly IConfigurationSection _smtpConfig;
        private readonly ILogger<SmtpAuthMailService> _logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ConfirmationMailLinkProvider provider;

        public SmtpAuthMailService(IConfiguration config, ILogger<SmtpAuthMailService> logger, IHttpContextAccessor httpContextAccessor, ConfirmationMailLinkProvider provider)
        {
            _smtpConfig = config.GetSection("Smtp");
            _logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.provider = provider;
        }

        public async Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken)
        {
            var currentRequest = httpContextAccessor.HttpContext.Request;
            var url = provider.GetConfirmationMailLink(toEmail, mailConfirmationToken, currentRequest.Scheme, currentRequest.Host.ToString());

            var client = new SmtpClient(_smtpConfig["Host"], int.Parse(_smtpConfig["Port"]));
            try
            {
                await client.SendMailAsync(_smtpConfig["From"], toEmail, "Validate your token", url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to confirmation token to {toEmail}");
                return false;
            }

            return true;
        }
    }
}
