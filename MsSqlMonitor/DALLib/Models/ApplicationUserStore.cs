using DALLib.EF;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DALLib.Models
{
    public class ApplicationUserStore : UserStore<User, Role, int,
    UserLogin, UserRole, UserClaim>
    {
        public ApplicationUserStore(MsSqlMonitorEntities context): base(context)
        {
        }
    }
}
