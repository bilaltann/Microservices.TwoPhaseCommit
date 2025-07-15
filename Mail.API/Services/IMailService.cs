namespace Mail.API.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string tomail,string subject,string body);
    }
}
