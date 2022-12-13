
using HealthCare_EmailService;

namespace HealthCare_Notifications
{
    public interface IEmailSender  
    {
        void SendEmail(Message message);
    }
}