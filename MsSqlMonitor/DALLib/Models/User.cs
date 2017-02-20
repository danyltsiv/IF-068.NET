using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DALLib.Models
{
    [Table("Users")]
    public partial class User: IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public virtual ICollection<Assign> Assigns { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}
