using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.ViewModels
{
    public class InstanceViewModel
    {
        public int? Id { get; set; }

        public string ServerName { get; set; }

        public string InstanceName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Status { get; set; }

        public bool? IsWindowsAuthentication { get; set; }

        public string OSVersion { get; set; }

        public byte? CpuCount { get; set; }

        public int? Memory { get; set; }

        public bool? IsOK { get; set; }

        public bool? IsDeleted { get; set; }

        public string Version { get; set; }

        public List<InstanceLoginViewModel> Logins { get; set; } = new List<InstanceLoginViewModel>();

        public List<InstanceRoleViewModel> Roles { get; set; } = new List<InstanceRoleViewModel>();

        #region These are collected from different table only for users and guests

        public string Alias { get; set; }

        public bool? IsHidden { get; set; }

        #endregion

        #region These are 'Count' values from associated tables

        public int? DatabasesCount { get; set; }

        #endregion
    }
}
