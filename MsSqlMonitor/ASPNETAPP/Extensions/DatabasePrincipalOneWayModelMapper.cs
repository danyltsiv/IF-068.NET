using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAPP.ViewModels;

namespace ASPNETAPP.Extensions
{
    public static class DatabasePrincipalOneWayModelMapper
    {
        public static DatabaseUserViewModel ToViewModel(this DbUser user)
        {
            return new DatabaseUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Type = user.Type,
                AssignedRoles = user.Roles.Select(role => role.Id).ToList()
            };
        }

        public static DatabaseRoleViewModel ToViewModel(this DbRole role)
        {
            return new DatabaseRoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                Type = role.Type,
                AssignedUsers = role.Users.Select(user => user.Id).ToList()
            };
        }

        public static DatabaseRoleViewModel ApplyPermissions(this DatabaseRoleViewModel role, IEnumerable<DbPermission> permissions)
        {
            role.Permissions = (from p in permissions select p.ToViewModel()).ToList();
            return role;
        }

        public static DatabaseUserViewModel ApplyPermissions(this DatabaseUserViewModel user, IEnumerable<DbPermission> permissions)
        {
            user.Permissions = (from p in permissions select p.ToViewModel()).ToList();
            return user;
        }

        public static DatabaseRoleViewModel AddUsersWithPermissions(this DatabaseRoleViewModel role, IEnumerable<DbUser> roles)
        {
            role.Users = (from r in roles select r.ToViewModel().ApplyPermissions(r.Permissions.ToList())).ToList();
            return role;
        }

        public static DatabaseUserViewModel AddRolesWithPermissions(this DatabaseUserViewModel user, IEnumerable<DbRole> roles)
        {
            user.Roles = (from r in roles select r.ToViewModel().ApplyPermissions(r.Permissions.ToList())).ToList();
            return user;
        }
    }
}
