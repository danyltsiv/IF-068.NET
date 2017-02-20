using DALLib.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.ComponentModel.DataAnnotations;

namespace DALLib.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
    public class UserRole : IdentityUserRole<int>
    {
    }
    public class UserClaim : IdentityUserClaim<int>
    {
    }
    public class UserLogin : IdentityUserLogin<int>
    {
    }
    public class Role : IdentityRole<int, UserRole>
    {
        public Role() { }
        public Role(string name) { Name = name; }
    }
    public class UserStore : UserStore<User, Role, int,
        UserLogin, UserRole, UserClaim>
    {
        public UserStore(MsSqlMonitorEntities context) : base(context)
        {
        }
    }
    public class RoleStore : RoleStore<Role, int, UserRole>
    {
        public RoleStore(MsSqlMonitorEntities context) : base(context)
        {
        }
    }
    public class ApplicationUserManager : UserManager<User, int>
    {
        public ApplicationUserManager(IUserStore<User, int> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<MsSqlMonitorEntities>();
            var appUserManager = new ApplicationUserManager(new UserStore(context.Get<MsSqlMonitorEntities>()));

            return appUserManager;
        }
    }
    public class ApplicationRoleManager : RoleManager<Role, int>
    {
        public ApplicationRoleManager(IRoleStore<Role, int> roleStore)
            : base(roleStore)
        {
        }
        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(
                new RoleStore(context.Get<MsSqlMonitorEntities>()));
        }
    }
    public class ApplicationSignInManager : SignInManager<User, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}