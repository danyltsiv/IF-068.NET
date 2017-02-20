using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.ViewModels
{
    public class DatabaseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Size { get; set; }

        public DateTime CreateDate { get; set; }

        public List<DatabaseUserViewModel> Users { get; set; } = new List<DatabaseUserViewModel>();

        public List<DatabaseRoleViewModel> Roles { get; set; } = new List<DatabaseRoleViewModel>();
    }
}
