using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_ModelView
{
    public class AppointmentDataViewModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Day { get; set; }
        public int NumberOfPatients { get; set; }
    }
}
