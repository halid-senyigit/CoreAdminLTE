using System;
using System.Collections.Generic;

namespace CoreAdminLTE.Data
{
    public class UserRoleRel
    {
        public int UserRoleRelID { get; set; }


        public int UserID { get; set; }
        public virtual User User { get; set; }


        public int RoleID { get; set; }
        public virtual Role Role { get; set; }

    }
}
