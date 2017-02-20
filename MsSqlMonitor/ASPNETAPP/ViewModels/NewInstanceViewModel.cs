using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETAPP.ViewModels
{
    public class NewInstanceViewModel
    {
        public int? Id { get; set; }

        public string ServerName { get; set; }

        public string InstanceName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool? IsWindowsAuthentication { get; set; }

    }
}