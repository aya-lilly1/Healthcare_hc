using System;
using System.Collections.Generic;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class Patient :User
    {
       // public int Id { get; set; }
        public int PatientId { get; set; }

        public virtual User PatientNavigation { get; set; }
        public virtual PatientAppointment PatientAppointment { get; set; }
    }
}
