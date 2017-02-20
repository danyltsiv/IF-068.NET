using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Contracts
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> FindByIdAsync(int id);
    }
}
