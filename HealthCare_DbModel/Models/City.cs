using System;
using System.Collections.Generic;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Archived { get; set; }
        public virtual State State { get; set; }
        public virtual User User { get; set; }
    }
}
