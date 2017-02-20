using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.ViewModels
{
    public class DatabaseUserViewModel : DatabasePrincipalViewModel
    {
        public List<int> AssignedRoles { get; set; } = new List<int>();

        public List<DatabaseRoleViewModel> Roles { get; set; } = new List<DatabaseRoleViewModel>();
    }
}
