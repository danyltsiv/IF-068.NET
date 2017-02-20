using System.Linq;
using DALLib.Models;
using DALLib.EF;
using DALLib.Contracts;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DALLib.Repos
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        const int PAGE_SIZE = 5;

        private IAssignRepository assignRepository;
        private IInstanceRepository instanceRepository;
        private ApplicationUserManager userManager;
        MsSqlMonitorEntities context;

        const string DEFAULT_USER = "Admin";
        readonly string DEFAULT_PSWD = $"\u00106$358psrutb";
        const string DEFAULT_ROLE = "Admin";
        const char DEFAULT_KEY = 'A';

        public UserRepository(MsSqlMonitorEntities context, IAssignRepository assignRepository
            , IInstanceRepository instanceRepository, ApplicationUserManager userManager) : base(context)
        {
            this.assignRepository = assignRepository;
            this.instanceRepository = instanceRepository;
            this.context = context;
            this.userManager = userManager;
       
            CheckIsAnyUser();
        }

        public  void CheckIsAnyUser()
        {
            var arr = userManager.Users.ToArray<User>();

 
            if (arr.Length == 0)
            {
                CreateDefaultUser();
                return;
            }
        }


        private string GetPswd(string text)
        {
            char[] array = text.ToCharArray();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (char)(array[i] ^ DEFAULT_KEY);
            }

            return new string(array);
        }

        private  void CreateDefaultUser()
        {
 
            User user = new User();
            user.UserName = DEFAULT_USER;

            var roleStore = new RoleStore<Role, int, UserRole>(context);
            ApplicationRoleManager roleManager = new ApplicationRoleManager(roleStore);

            if (!roleManager.RoleExists(DEFAULT_ROLE))
            {
                var role = new Role();
                role.Name = DEFAULT_ROLE;
                roleManager.Create<Role, int>(role);
            }

           // ApplicationUserManager manager = new ApplicationUserManager(new UserStore(new MsSqlMonitorEntities()));
            var result = userManager.Create(user, GetPswd(DEFAULT_PSWD));

            if (result.Succeeded)
            {
                userManager.AddToRole(user.Id, DEFAULT_ROLE);
            
            }
        }

        public async Task<User> FindUser(string userName, string password)
        {
           

            User user = await userManager.FindAsync(userName, password);

            return user;
        }

        public User GetByCredential(string userName)
        {
            return table.FirstOrDefault(g => g.UserName == userName);
        }

        public async Task<User> GetByCredentialAsync(string userName)
        {
            return await table.FirstOrDefaultAsync(g => g.UserName == userName);
        }

        public bool IsLoginExist(string userName)
        {
            return table.Any(g => g.UserName == userName);
        }

        public async Task<bool> IsLoginExistAsync(string userName)
        {
            return await table.AnyAsync(g => g.UserName == userName);
        }

        public bool IsLoginExist(User user)
        {
            return table.Any(g => g.UserName == user.UserName && user.Id != g.Id);
        }

        public async Task<bool> IsLoginExistAsync(User user)
        {
            return await table.AnyAsync(g => g.UserName == user.UserName && user.Id != g.Id);
        }

        public IEnumerable<Instance> GetAssignedInstances(int userId)
        {
            List<int> assignedInstanceId = GetAssignedInstanceId(userId);

            return instanceRepository.GetAll().Where(g => assignedInstanceId.Any(i => i == g.Id));
        }

        public async Task<IEnumerable<Instance>> GetAssignedInstancesAsync(int userId)
        {
            List<int> assignedInstanceId = GetAssignedInstanceId(userId);

            var instances = await instanceRepository.GetAllAsync();
            return instances.Where(g => assignedInstanceId.Any(i => i == g.Id));
        }

        public async Task<PaginatedInstances> GetAssignedInstancesAsync(int userId,int page = 0, String namefilter = "", String serverNamefilter = "", String versionFilter = "",string sqlversions="")
        {
            var result = new PaginatedInstances();

            try
            {
                List<int> assignedInstanceId = GetAssignedInstanceId(userId);
                var listAll = await instanceRepository.GetAllAsync();

                var instances = listAll.Where(g => assignedInstanceId.Any(i => i == g.Id) && CheckInstance(g, namefilter, serverNamefilter, versionFilter, sqlversions));
   

                int pageCount = instances.Count<Instance>() / PAGE_SIZE;
                if (pageCount * PAGE_SIZE < instances.Count<Instance>()) pageCount++;

                if (page > pageCount) page = pageCount;
                if (page < 0) page = 0;

                int startElement = page * PAGE_SIZE;

                var resultList = instances.Skip<Instance>(startElement).Take<Instance>(PAGE_SIZE);



                result.List = resultList;
                result.Page = page;
                result.PageCount = pageCount;
                result.PageSize = PAGE_SIZE;
                result.Versions = listAll.Where(i => i.InstanceVersion != null).Select<Instance, String>(i => i.InstanceVersion.Version.Split('.')[0]).ToList().Distinct();

            }
            catch (Exception e)
            {
                result.Error = e.Message;
            }

            return result;
        }

        public async Task<PaginatedInstances> GetNotAssignedInstancesAsync(int userId, int page = 0, String namefilter = "", String serverNamefilter = "", String versionFilter = "", string sqlversions = "")
        {
            var result = new PaginatedInstances();
            try
            {
                List<int> assignedInstanceId = GetAssignedInstanceId(userId);
                var listAll = await instanceRepository.GetAllAsync();
                var instances = listAll.Where(i => !assignedInstanceId.Contains(i.Id))
                                        .Distinct();

                int pageCount = instances.Count<Instance>() / PAGE_SIZE;
                if (pageCount * PAGE_SIZE < instances.Count<Instance>()) pageCount++;
                if (page > pageCount) page = pageCount;
                if (page < 0) page = 0;
                int startElement = page * PAGE_SIZE;
                var resultList = instances.Skip<Instance>(startElement).Take<Instance>(PAGE_SIZE);
                result.List = resultList;
                result.Page = page;
                result.PageCount = pageCount;
                result.PageSize = PAGE_SIZE;
                result.Versions = listAll.Where(i => i.InstanceVersion != null).Select<Instance, String>(i => i.InstanceVersion.Version.Split('.')[0]).ToList().Distinct();

            }
            catch (Exception e)
            {
                result.Error = e.Message;
            }

            return result;
        }

        public bool CheckInstance(Instance instance, string nameFilter, string serverNameFilter, string versionFilter,string sqlversion)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(nameFilter)) result = result && instance.InstanceName.Contains(nameFilter);
            if (!string.IsNullOrEmpty(serverNameFilter)) result = result && instance.ServerName.Contains(nameFilter);
            if (!string.IsNullOrEmpty(versionFilter) && instance.InstanceVersion!=null) result = result && instance.InstanceVersion.Version.ToString().Contains(versionFilter);

            if (instance.InstanceVersion != null)
            {
                SQLVersion version = SQLVersions.GetSQLVersionFromString(instance.InstanceVersion.Version);
                result = result && SQLVersions.IsVersionInlist(version, sqlversion);
            }

      
            return result;
        }

        private List<int> GetAssignedInstanceId(int userId)
        {
            return assignRepository.GetUserAssigns(userId).Select(g => g.InstanceId).ToList();
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AssignSingleRoleAsync(int userId, string role)
        {
            if (!String.IsNullOrEmpty(role) && context.Roles.Any(g => g.Name.ToLower() == role.ToLower()))
            {
                var roles = context.Roles.Where(g => g.Users.Any(u => u.UserId == userId)).ToList();
                foreach (var rol in roles)
                {
                    await userManager.RemoveFromRoleAsync(userId, rol.Name);
                }

                var result = await userManager.AddToRoleAsync(userId, role);

                if (result.Succeeded) return true;
            } 

            return false;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            var removeResult = await userManager.RemovePasswordAsync(userId);
            var addResult = await userManager.AddPasswordAsync(userId, newPassword);

            if (removeResult.Succeeded && addResult.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<User> CreateAsync(User user, string password, string role)
        {
  
                await CheckRole(role);

                ApplicationUserManager manager = new ApplicationUserManager(new UserStore(new MsSqlMonitorEntities()));
                var result = await manager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await manager.AddToRoleAsync(user.Id, role);
                    return user;
                }
                else
                {
                    return null;
                }

        }

        public async Task CheckRole(string roleName)
        {

         
            var roleStore = new RoleStore<Role, int, UserRole>(context);
            ApplicationRoleManager roleManager = new ApplicationRoleManager(roleStore);

            if (! await roleManager.RoleExistsAsync(roleName))
            {
                var role = new Role();
                role.Name = roleName;
                roleManager.Create<Role,int>(role);
            }

  
        }

        public override async Task<User> DeleteAsync(int id)
        {
            if ( await IsItlastAdmin(id)) return null;

            User userToDelete = await GetAsync(id);

            var result = await userManager.DeleteAsync(userToDelete);

            if (result.Succeeded)
            {
                return userToDelete;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool>  IsItlastAdmin(int id)
        {
            var roleStore = new RoleStore<Role, int, UserRole>(context);
            ApplicationRoleManager roleManager = new ApplicationRoleManager(roleStore);

            Role adminRole = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name.Equals("Admin"));
            if (adminRole == null) return false;


  
            var result = userManager.Users.Any(user => user.Id != id && user.Roles.Any(i => i.UserId == user.Id && i.RoleId == adminRole.Id));

            return !result;
        }

        public async override Task<User> GetAsync(int id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await userManager.FindByNameAsync(username);
        }
    }
}