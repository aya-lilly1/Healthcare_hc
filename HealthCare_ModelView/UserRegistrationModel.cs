using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_ModelView
{
    public class UserRegistrationModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [DefaultValue("")]
        public string Image { get; set; }
        public string ImageString { get; set; }
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
    }
}
