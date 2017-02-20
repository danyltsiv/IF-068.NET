using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.ViewModels
{
    public class DatabaseRoleViewModel : DatabasePrincipalViewModel
    {
        public List<int> AssignedUsers { get; set; } = new List<int>();

        public List<DatabaseUserViewModel> Users { get; set; } = new List<DatabaseUserViewModel>();
    }
}
