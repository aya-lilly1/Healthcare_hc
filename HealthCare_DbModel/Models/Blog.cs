using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class Blog
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public bool Archived { get; set; }

        [Timestamp]
        public DateTime CreatedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }

        public virtual Doctor Creator { get; set; }
    }
}
