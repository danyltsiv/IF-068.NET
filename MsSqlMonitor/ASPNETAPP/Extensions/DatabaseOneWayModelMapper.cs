using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAPP.ViewModels;

namespace ASPNETAPP.Extensions
{
    public static class DatabaseOneWayModelMapper
    {
        public static DatabaseViewModel ToViewModel(this Database database)
        {
            return new DatabaseViewModel()
            {
                Id = database.Id,
                Name = database.Name,
                Size = database.Size,
                CreateDate = database.CreateDate
            };
        }

        public static DatabaseViewModel SetUsers(this DatabaseViewModel database, IEnumerable<DatabaseUserViewModel> users)
        {
            database.Users = users.ToList();
            return database;
        }

        public static DatabaseViewModel SetRoles(this DatabaseViewModel database, IEnumerable<DatabaseRoleViewModel> roles)
        {
            database.Roles = roles.ToList();
            return database;
        }
    }
}
