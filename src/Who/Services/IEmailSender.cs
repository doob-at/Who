using System.Threading.Tasks;

namespace doob.Who.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
