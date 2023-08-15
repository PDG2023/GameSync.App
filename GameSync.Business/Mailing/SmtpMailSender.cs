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

            var client = new SmtpClient(_smtpConfig["Host"], int.Parse(_smtpConfig["Port"]));
            var mailBody = new MailMessage(_smtpConfig["From"], recipient)
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
