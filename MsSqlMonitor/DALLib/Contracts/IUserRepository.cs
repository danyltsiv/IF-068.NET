using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindUser(string userName, string password);
        User GetByCredential(string login);
        Task<User> GetByCredentialAsync(string login);
        bool IsLoginExist(string login);
        Task<bool> IsLoginExistAsync(string login);
        bool IsLoginExist(User user);
        Task<bool> IsLoginExistAsync(User user);
        IEnumerable<Instance> GetAssignedInstances(int userId);
        Task<IEnumerable<Instance>> GetAssignedInstancesAsync(int userId);
        Task<PaginatedInstances> GetAssignedInstancesAsync(int userId, int page, String namefilter, String serverNamefilter, String versionFilter,String sqlversions);
        Task<PaginatedInstances> GetNotAssignedInstancesAsync(int userId, int page, String namefilter, String serverNamefilter, String versionFilter, String sqlversions);
        Task<User> CreateAsync(User user, string password, string role);
        Task<User> DeleteAsync(int id);
        Task<bool> UpdateAsync(User user);
        Task<bool> AssignSingleRoleAsync(int userId, string role);
        Task<bool> ChangePasswordAsync(int userId, string newPassword);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
