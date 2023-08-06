namespace GameSync.Business.Auth
{
    public class SmtpAuthMailService : IAuthMailService
    {
        public Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken)
        {
            throw new NotImplementedException();
        }
    }
}
