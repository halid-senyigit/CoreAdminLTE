namespace CoreAdminLTE.Services.Interfaces
{

    public interface IEmailService
    {
        void SendMail(EmailModel emailModel);
    }
}