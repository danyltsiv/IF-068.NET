using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.ViewModels
{
    public class InstanceRoleViewModel : InstancePrincipalViewModel
    {
        public List<InstanceLoginViewModel> Logins { get; set; }

        public List<int> AssignedLoginsIds { get; set; }
    }
}
