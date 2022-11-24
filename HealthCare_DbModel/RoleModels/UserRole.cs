using System;

namespace Healthcare_hc.Models.RoleModels
{
    public class UserRole
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime LastUpdatedUTC { get; set; }
        public bool Archived { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
