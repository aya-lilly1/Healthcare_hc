using System;
using System.Collections.Generic;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class Doctor : User
    {
        public Doctor()
        {
            Blogs = new HashSet<Blog>();
        }

        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int ClinicId { get; set; }
        public int DepartmentId { get; set; }
        public bool Status { get; set; }

        public virtual Clinic Clinic { get; set; }
        public virtual Department Department { get; set; }
        public virtual User DoctorNavigation { get; set; }
        public virtual DoctorAppointment DoctorAppointment { get; set; }
        public virtual PatientAppointment PatientAppointment { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
