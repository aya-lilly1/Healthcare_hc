using System;
using System.Collections.Generic;

namespace Healthcare_hc.Models.RoleModels
{
    public class Module
    {
        public Module()
        {
            UserPermissions = new HashSet<Permission>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime LastUpdatedUTC { get; set; }
        public bool Archived { get; set; }

        public ICollection<Permission> UserPermissions { get; set; }
    }
}
