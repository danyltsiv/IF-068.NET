using SQLInfoCollectionService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.Contracts
{
    public interface IResourceManager
    {
        string GetInstanceDetailsScript(string version);

        string GetInstanceRolesScript(string version);

        string GetInstanceLoginsScript(string version);

        string GetInstancePermissionsScript(string version);

        string GetDatabasesScript(string version);

        string GetDbRolesScript(string version);

        string GetDbUsersScript(string version);

        string GetDbPermissionsScript(string version);
    }
}
