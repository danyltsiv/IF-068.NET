using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAPP.ViewModels;

namespace ASPNETAPP.Extensions
{
    public static class UserMapper
    {
        public static UserViewModel ToViewModel(this User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Roles = user.Roles
            };
        }

        public static IEnumerable<UserViewModel> ToViewModelList(this IEnumerable<User> userList)
        {
            List<UserViewModel> mapped = new List<UserViewModel>();

            foreach (var user in userList)
            {
                mapped.Add(user.ToViewModel());
            }

            return mapped.AsEnumerable();
        }

        //public static User ToDALModel(this UserViewModel user)
        //{
        //    return new User
        //    {
        //        Id = user.Id,
        //        UserName = user.Login,
        //        Roles = user.Roles
        //    };
        //}

        //public static IEnumerable<User> ToDALModelList(this IEnumerable<UserViewModel> userList)
        //{
        //    List<User> mapped = new List<User>();

        //    foreach (var user in userList)
        //    {
        //        mapped.Add(new User
        //        {
        //            Id = user.Id,
        //            Login = user.Login,
        //            Role = user.Role
        //        });
        //    }

        //    return mapped.AsEnumerable();
        //}
    }
}
