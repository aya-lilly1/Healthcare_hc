using Healthcare_hc.Models.RoleModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class User
    {
        public User()
        {
        
            UserRoles = new HashSet<UserRole>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Phone { get; set; }
        public bool Archived { get; set; }
        [Timestamp]
        public DateTime CreatedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsDoctor { get; set; }
        public bool EmailConfirmed { get; set; }
        
        public string ConfirmationLink { get; set; }
        public virtual City City { get; set; }
        public virtual State State { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
       
        //public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual PatientAppointment PatientAppointment { get; set; }
        public virtual DoctorAppointment DoctorAppointment { get; set; }

    }
}
