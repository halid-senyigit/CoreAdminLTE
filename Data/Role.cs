using System;
using System.Collections.Generic;

namespace CoreAdminLTE.Data
{
    public class Role
    {
        public int RoleID { get; set; }

        public string Name { get; set; }


        public virtual ICollection<UserRoleRel> UserRoleRels { get; set; }

    }
}
