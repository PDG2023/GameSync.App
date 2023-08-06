namespace GameSync.Business.Auth
{
    public class SmtpAuthMailService : IAuthMailService
    {
        public Task<bool> SendEmailConfirmation(string toEmail, string mailConfirmationToken)
        {
            throw new NotImplementedException();
        }
    }
}
