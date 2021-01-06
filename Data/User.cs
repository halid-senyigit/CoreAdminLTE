using System;
using System.Collections.Generic;

namespace CoreAdminLTE.Data
{
    public class User
    {
        public int UserID { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ProfilePicture { get; set; }

        public string PassResetCode { get; set; }

        public DateTime ResetCodeExpireDate { get; set; }

        public virtual ICollection<UserRoleRel> UserRoleRels { get; set; }
    }
}
