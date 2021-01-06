using System.Threading.Tasks;

namespace CoreAdminLTE.Services.Interfaces
{

    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}