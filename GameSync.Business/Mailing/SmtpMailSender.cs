using GameSync.Business.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;

namespace GameSync.Business.Mailing
{
    public class SmtpMailSender : IMailSender
    {
        private readonly IConfigurationSection _smtpConfig;
        private readonly ILogger<SmtpMailSender> _logger;

        public SmtpMailSender(IConfiguration config, ILogger<SmtpMailSender> logger)
        {
            _smtpConfig = config.GetSection("Smtp");
            _logger = logger;
        }

        public async Task<bool> SendMailAsync(string subject, string htmlBody, string recipient)
        {

            var from = _smtpConfig["From"] ?? throw new Exception("SMTP From not configured");
            var host = _smtpConfig["Host"] ?? throw new Exception("Host is not configured");
            var port = _smtpConfig["Port"] ?? throw new Exception("SMTP Port not configured");

            var client = new SmtpClient(host, int.Parse(port));
            var mailBody = new MailMessage(from, recipient)
            {
                IsBodyHtml = true,
                Body = htmlBody,
                Subject = subject
            };

            try
            {
                await client.SendMailAsync(mailBody);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to send the mail with a smpt client");
                return false;
            }
        }
    }
}
