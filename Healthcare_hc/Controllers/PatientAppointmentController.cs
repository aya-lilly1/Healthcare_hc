using HealtCare_Core.Managers.Interfaces;
using HealthCare_ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Healthcare_hc.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    public class PatientAppointmentController : ApiBaseController
    {
        private IPatiantManager _patientManager;
        private readonly ILogger<PatientAppointmentController> _logger;
        public PatientAppointmentController(ILogger<PatientAppointmentController> logger,
                             IPatiantManager patientManager)
        {
            _logger = logger;
            _patientManager = patientManager;
        }
        [Route("api/v{version:apiVersion}/Appointment/CreateAppointment")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult CreateAppointment( PatientAppointmentModelView appointmentMV, int idDoctor)
        {
            var res = _patientManager.CreateAppointment(LoggedInUser, appointmentMV, idDoctor);
            return Ok(res);
        }
    }
}