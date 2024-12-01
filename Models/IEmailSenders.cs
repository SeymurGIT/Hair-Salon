using System.Threading.Tasks;

namespace HairSalon.Models
{
    public interface IEmailSenders
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
