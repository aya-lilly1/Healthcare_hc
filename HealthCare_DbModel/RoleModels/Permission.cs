using System;
using System.Collections.Generic;

namespace Healthcare_hc.Models.RoleModels
{
    public class Permission
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }

        public int Id { get; set; }

        public int ModuleId { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public DateTime CreatedUTC { get; set; }

        public DateTime LastUpdatedUTC { get; set; }

        public bool Archived { get; set; }

        public virtual Module Module { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
