using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAPP.ViewModels;

namespace ASPNETAPP.Extensions
{
    public static class PermissionOneWayModelMapper
    {
        public static PermissionViewModel ToViewModel(this InstPermission permission)
        {
            return new PermissionViewModel()
            {
                Name = permission.Name,
                State = permission.State
            };
        }

        public static PermissionViewModel ToViewModel(this DbPermission permission)
        {
            return new PermissionViewModel()
            {
                Name = permission.Name,
                State = permission.State
            };
        }
    }
}
