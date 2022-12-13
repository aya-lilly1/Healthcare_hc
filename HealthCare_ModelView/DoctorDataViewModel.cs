using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_ModelView
{
    public class DoctorDataViewModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string ClinicName { get; set; }
        
        public string Image { get; set; }
        public string ImageString { get; set; }
        public string Address { get; set; }



    }
}
