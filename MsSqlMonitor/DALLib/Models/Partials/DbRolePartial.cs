using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    public partial class DbRole : DbPrincipal
    {
        public void RefuseAllUsers()
        {
            this.Users.ToList().ToList().ForEach(u => u.Roles.Remove(this));
        }

        public void RefuseAllPermissions()
        {
            this.Permissions.ToList().ForEach(perm => perm.Principals.Remove(this));
        }

        public void RefuseAllDependencies()
        {
            RefuseAllUsers();
            RefuseAllPermissions();
        }
    }
}
