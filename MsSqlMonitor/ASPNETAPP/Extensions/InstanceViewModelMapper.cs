using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALLib;
using ASPNETAPP.ViewModels;
using DALLib.Models;

namespace ASPNETAPP.Extensions
{
    public static class InstanceViewModelMapper
    {
        public static InstanceViewModel ToViewModel(this Instance instance)
        {
            return new InstanceViewModel()
            {
                Id = instance.Id,
                ServerName = instance.ServerName,
                InstanceName = instance.InstanceName,
                Status = instance.Status.ToString(),
                OSVersion = instance.OSVersion,
                CpuCount = instance.CpuCount,
                Memory = instance.Memory,
                IsOK = instance.IsOK,
                IsDeleted = instance.IsDeleted,
                DatabasesCount = instance.Databases?.Count,
                Version = instance.InstanceVersion != null ? $"{instance.InstanceVersion.Version}" : String.Empty
            };
        }

        public static Instance ToEntity(this InstanceViewModel viewModel)
        {
            return new Instance(Convert.ToInt32(viewModel.Id))
            {
                ServerName = viewModel.ServerName,
                InstanceName = viewModel.InstanceName,
                Login = viewModel.Login,
                Password = viewModel.Password,
                Authentication = Convert.ToBoolean(viewModel.IsWindowsAuthentication) ? AuthenticationType.Windows : AuthenticationType.Sql
            };
        }

        public static InstanceViewModel ApplyCustomization(this InstanceViewModel instance, Assign assigns)
        {
            instance.IsHidden = assigns.IsHidden;
            instance.Alias = assigns.Alias;
            return instance;
        }

        public static InstanceViewModel SetLogins(this InstanceViewModel instance, IEnumerable<InstanceLoginViewModel> logins)
        {
            instance.Logins = logins.ToList();
            return instance;
        }

        public static InstanceViewModel SetRoles(this InstanceViewModel instance, IEnumerable<InstanceRoleViewModel> roles)
        {
            instance.Roles = roles.ToList();
            return instance;
        }
    }
}