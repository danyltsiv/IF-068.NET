using SQLInfoCollectionService.Contracts;

using SQLInfoCollectorService.Properties;
using System;

namespace SQLInfoCollectionService.Entities
{
    public class ResourceManager : IResourceManager
    {
        private const string SQL_SERVER_2005 = "9";
        private const string SQL_SERVER_2005v2 = "09";
        private const string SQL_SERVER_2008 = "10";
        private const string SQL_SERVER_2012 = "11";
        private const string SQL_SERVER_2014 = "12";
        private const string SQL_SERVER_2016 = "13";

        public string GetDatabasesScript(string version)
        {
            string resourceName = "databases";

            return Resources.ResourceManager
             .GetString(GenerateResourceFullName(resourceName, version));
        }

        public string GetDbPermissionsScript(string version)
        {
            string resourceName = "dbPermissions";

            return Resources.ResourceManager
                .GetString(GenerateResourceFullName(resourceName, version));
        }

        public string GetDbUsersScript(string version)
        {
            string resourceName = "dbUsers";

            return Resources.ResourceManager
                .GetString(GenerateResourceFullName(resourceName, version));
        }

        public string GetDbRolesScript(string version)
        {
            string resourceName = "dbRoles";

            return Resources.ResourceManager
                .GetString(GenerateResourceFullName(resourceName, version));
        }

        public string GetInstanceDetailsScript(string version)
        {
            string resourceName = "instanceDetails";

            return Resources.ResourceManager
                .GetString(GenerateResourceFullName(resourceName, version));
        }

        public string GetInstancePermissionsScript(string version)
        {
            string resourceName = "instancePermissions";

            return Resources.ResourceManager
                .GetString(GenerateResourceFullName(resourceName, version));
        }

        public string GetInstanceLoginsScript(string version)
        {
            string resourceName = "instanceLogins";

            return Resources.ResourceManager
                .GetString(GenerateResourceFullName(resourceName, version));
        }

        public string GetInstanceRolesScript(string version)
        {
            string resourceName = "instanceRoles";

            return Resources.ResourceManager
                .GetString(GenerateResourceFullName(resourceName, version));
        }

        public string GetPoftfix(string version)
        {
            string major = version.Split('.')[0];

            switch (major)
            {
                case SQL_SERVER_2005:
                case SQL_SERVER_2005v2:
                case SQL_SERVER_2008:
                case SQL_SERVER_2012:
                case SQL_SERVER_2014:
                case SQL_SERVER_2016:
                    return  "2008";
                default:
                    return null;//Throwing WrongVersionException here
            }
        }

        private string GenerateResourceFullName(string resourceName, string version)
        {
            return String.Concat(resourceName, GetPoftfix(version));
        }
    }
}
