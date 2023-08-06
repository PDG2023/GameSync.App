using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace GameSync.Business.Auth
{
    public class SmtpAuthMailService : IAuthMailService
    {
        private readonly IConfigurationSection _config;
        private readonly ILogger<SmtpAuthMailService> _logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SmtpAuthMailService(IConfiguration config, ILogger<SmtpAuthMailService> logger, IHttpContextAccessor httpContextAccessor)
        {
            this._config = config.GetSection("Smtp");
            this._logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken)
        {
            var currentRequest = httpContextAccessor.HttpContext.Request;
            var url = $"{currentRequest.Scheme}://{currentRequest.Host}/api/users/confirm?token={mailConfirmationToken}&email={toEmail}";

            var client = new SmtpClient(_config["Host"], int.Parse(_config["Port"]));
            try
            {
                await client.SendMailAsync(_config["From"], toEmail, "Validate your token", $"""<a href="{url}">Click here to validate</a>""");
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
