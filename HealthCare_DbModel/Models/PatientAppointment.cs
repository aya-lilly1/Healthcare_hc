using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class PatientAppointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Day { get; set; }
        public DateTime Time { get; set; }
        public bool Archived { get; set; }
        [Timestamp]
        public DateTime CreatedDate { get; set; }

 
   //     public virtual Doctor Doctor { get; set; }
        public virtual User User { get; set; }
    }
}
