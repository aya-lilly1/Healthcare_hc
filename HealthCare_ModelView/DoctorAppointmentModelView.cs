using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_ModelView
{
    public class DoctorAppointmentModelView
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfPatients { get; set; }
    }
}
