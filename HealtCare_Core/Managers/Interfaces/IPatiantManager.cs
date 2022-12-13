using HealthCare_Core.Managers.Interfaces;
using HealthCare_ModelView;

namespace HealtCare_Core.Managers.Interfaces
{
    public interface IPatiantManager : IManager
    {
        PatientAppointmentModelView CreateAppointment(UserModelView currentUser, PatientAppointmentModelView appointmentMV, int idDoctor);
    }
}
