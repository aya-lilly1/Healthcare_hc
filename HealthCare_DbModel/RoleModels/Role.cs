using System;
using System.Collections.Generic;

namespace Healthcare_hc.Models.RoleModels
{
    public class Role
    {
        public Role()
        {
            RolePermissions = new HashSet<RolePermission>();
            UserRoles = new HashSet<UserRole>();
        } 

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedUTC { get; set; }

        public DateTime LastUpdatedUTC { get; set; }

        public bool Archived { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
