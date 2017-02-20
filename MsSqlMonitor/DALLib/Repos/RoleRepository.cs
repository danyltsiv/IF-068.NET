using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALLib.EF;
using DALLib.Contracts;

namespace DALLib.Repos
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private ApplicationRoleManager roleManager;

        public RoleRepository(MsSqlMonitorEntities context, ApplicationRoleManager roleManager) : base(context)
        {
            this.roleManager = roleManager;
        }

        public async Task<Role> FindByIdAsync(int id)
        {
            return await roleManager.FindByIdAsync(id);
        }
    }
}
