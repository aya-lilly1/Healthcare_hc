using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public bool Archived { get; set; }
        [Timestamp]
        public DateTime CreatedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
