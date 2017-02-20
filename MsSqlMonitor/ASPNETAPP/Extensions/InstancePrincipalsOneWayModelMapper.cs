using DALLib.Models;
using ASPNETAPP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.Extensions
{
    public static class InstancePrincipalsOneWayModelMapper
    {
        public static InstanceLoginViewModel ToViewModel(this InstLogin login)
        {
            return new InstanceLoginViewModel()
            {
                Id = login.Id,
                Name = login.Name,
                Type = login.Type,
                AssignedRolesIds = (from r in login.Roles select r.Id).ToList(),
                IsDisabled = login.IsDisabled
            };
        }

        public static InstanceRoleViewModel ToViewModel(this InstRole role)
        {
            return new InstanceRoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                Type = role.Type,
                AssignedLoginsIds = (from l in role.Logins select l.Id).ToList()
            };
        }

        public static InstanceRoleViewModel ApplyPermissions(this InstanceRoleViewModel role, IEnumerable<InstPermission> permissions)
        {
            role.Permissions = (from p in permissions select p.ToViewModel()).ToList();
            return role;
        }

        public static InstanceLoginViewModel ApplyPermissions(this InstanceLoginViewModel login, IEnumerable<InstPermission> permissions)
        {
            login.Permissions = (from p in permissions select p.ToViewModel()).ToList();
            return login;
        }

        public static InstanceRoleViewModel AddLoginsWithPermissions(this InstanceRoleViewModel role, IEnumerable<InstLogin> logins)
        {
            role.Logins = (from l in logins select l.ToViewModel().ApplyPermissions(l.Permissions.ToList())).ToList();
            return role;
        }

        public static InstanceLoginViewModel AddRolesWithPermissions(this InstanceLoginViewModel login, IEnumerable<InstRole> roles)
        {
            login.Roles = (from r in roles select r.ToViewModel().ApplyPermissions(r.Permissions.ToList())).ToList();
            return login;
        }
    }
}
