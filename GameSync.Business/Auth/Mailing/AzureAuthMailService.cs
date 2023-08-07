namespace GameSync.Business.Auth.Mailing
{
    public class AzureAuthMailService : IAuthMailService
    {
        public Task<bool> SendEmailConfirmationAsync(string toEmail, string mailConfirmationToken)
        {

        }
    }
}
