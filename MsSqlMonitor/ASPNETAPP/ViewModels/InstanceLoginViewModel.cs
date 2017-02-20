using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.ViewModels
{
    public class InstanceLoginViewModel : InstancePrincipalViewModel
    {
        public bool IsDisabled { get; set; }

        public List<InstanceRoleViewModel> Roles { get; set; }

        public List<int> AssignedRolesIds { get; set; }
    }
}
