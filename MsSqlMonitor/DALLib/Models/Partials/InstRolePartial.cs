using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    public partial class InstRole
    {
        public void RefuseAllLogins()
        {
            Logins.ToList().ForEach(il => il.Roles.Remove(this));
        }

        public void RefuseAllPermissions()
        {
            Permissions.ToList().ForEach(perm => perm.Principals.Remove(this));
        }

        public void RefuseAllDependencies()
        {
            RefuseAllLogins();
            RefuseAllPermissions();
        }
    }
}
