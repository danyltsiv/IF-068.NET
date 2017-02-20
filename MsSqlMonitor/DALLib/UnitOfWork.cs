using DALLib.Contracts;
using System;
using DALLib.Repos;
using DALLib.EF;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using DALLib.Models;

namespace DALLib
{
    public class UnitOfWork : IUnitOfWork
    {
        private MsSqlMonitorEntities context;
        private IAssignRepository assignRepository;
        private IDatabaseRepository databaseRepository;
        private IDbPermissionRepository dbPermissionRepository;
        private IInstanceRepository instanceRepository;
        private IInstPermissionRepository instPermissionRepository;
        private IUserRepository userRepository;
        private IRoleRepository roleRepository;
        private IJobTypeRepository jobTypeRepository;
        private ApplicationUserManager userManager;
        ApplicationRoleManager roleManager;
        private bool disposed = false;

        public UnitOfWork(MsSqlMonitorEntities context, ApplicationUserManager userMan, ApplicationRoleManager roleMan)
        {
            this.context = context;
            userManager = userMan;
            roleManager = roleMan;
        }

        public IAssignRepository Assigns
        {
            get
            {
                if (assignRepository == null)
                    assignRepository = new AssignRepository(context);
                return assignRepository;
            }
        }

        public IJobTypeRepository JobTypes
        {
            get
            {
                if (jobTypeRepository == null)
                    jobTypeRepository = new JobTypeRepository(context);
                return jobTypeRepository;
            }
        }

        public IDatabaseRepository Databases
        {
            get
            {
                if (databaseRepository == null)
                    databaseRepository = new DatabaseRepository(context);
                return databaseRepository;
            }
        }

        public IDbPermissionRepository DbPermissions
        {
            get
            {
                if (dbPermissionRepository == null)
                    dbPermissionRepository = new DbPermissionRepository(context);
                return dbPermissionRepository;
            }
        }

        public IInstanceRepository Instances
        {
            get
            {
                if (instanceRepository == null)
                    instanceRepository = new InstanceRepository(context);
                return instanceRepository;
            }
        }

        public IInstPermissionRepository InstPermissions
        {
            get
            {
                if (instPermissionRepository == null)
                    instPermissionRepository = new InstPermissionRepository(context);
                return instPermissionRepository;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(context, Assigns, Instances, 
                        new ApplicationUserManager(new UserStore(context)));
                return userRepository;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                if (roleRepository == null)
                    roleRepository = new RoleRepository(context, new ApplicationRoleManager(new RoleStore(context)));
                return roleRepository;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
