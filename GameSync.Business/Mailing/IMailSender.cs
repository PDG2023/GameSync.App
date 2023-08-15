using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSync.Business.Mailing
{
    public interface IMailSender
    {
        Task<bool> SendMailAsync(string subject, string htmlBody, string recipient);
    }
}
