using System;
using System.Collections.Generic;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Archived { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
