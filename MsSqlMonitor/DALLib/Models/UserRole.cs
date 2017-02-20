using Microsoft.AspNet.Identity.EntityFramework;

namespace DALLib.Models
{
    public class UserRole : IdentityUserRole<int>
    {
    }
    public class Role : IdentityRole<int, UserRole>
    {
        public Role() { }
        public Role(string name) { Name = name; }
    }
}
