using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.ViewModels
{
    public class DatabasePrincipalViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public List<PermissionViewModel> Permissions { get; set; }
    }
}
