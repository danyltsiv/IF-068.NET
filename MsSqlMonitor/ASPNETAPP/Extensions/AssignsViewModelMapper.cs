using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPNETAPP.ViewModels;

namespace ASPNETAPP.Extensions
{
    public static class AssignsViewModelMapper
    {
        public static void MergeWithInstance(this Assign assign, InstanceViewModel instance)
        {
            instance.IsHidden = assign.IsHidden;
            instance.Alias = assign.Alias;
        }

        public static void SeparateFromInstance(this Assign assign, InstanceViewModel instance)
        {
            assign.Alias = instance.Alias;
            assign.IsHidden = Convert.ToBoolean(instance.IsHidden);
        }
    }
}