using System.Threading.Tasks;

namespace doob.Who.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
