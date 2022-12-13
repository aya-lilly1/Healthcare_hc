using System;
using System.Collections.Generic;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class State
    {
        public State()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public bool Archived { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
