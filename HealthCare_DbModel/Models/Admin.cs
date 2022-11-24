using System;
using System.Collections.Generic;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class Admin :User
    {
        public int Id { get; set; }
        public int AdminId { get; set; }

        public virtual User AdminNavigation { get; set; }
    }
}
