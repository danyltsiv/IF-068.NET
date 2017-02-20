using Microsoft.Practices.Unity;
using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService;
using System.Data.SqlClient;
using DALLib.Impersonation;

namespace SQLInfoCollectionService.DataUpdating.HelpClasses
{
    using Entities;
    using System.Resources;

    public class InstanceInfoHelper
    {
        public InstanceInfo UpdateStatusOnly(Instance instance, IInstanceDataCollector collector)
        {
            return ParseInstanceInfo(new InstanceInfo(instance.Id), collector.GetInstanceInfo().Rows[0]);
        }

        public async Task<InstanceInfo> UpdateStatusOnlyAsync(Instance instance, IInstanceDataCollector collector)
        {
            return ParseInstanceInfo(
                new InstanceInfo(instance.Id), 
                (await collector.GetInstanceInfoAsync()).Rows[0]);
        }

        public InstanceInfo Update(Instance instance, IInstanceDataCollector collector)
        {
            // Get information about databases and their principals (inluding permissions).
            // Stars first because it has more data and more time to process it.
            DataTable databases = collector.GetDatabases(); // List of all databases on the instance with some basic information (size, date of creation).
            DataTable databaseRoles = collector.GetDatabaseRoles(); // List of all roles on all databases.
            DataTable databaseUsers = collector.GetDatabaseUsers(); // List of all users on all databases.
            DataTable databasePermissions = collector.GetDatabasePermissions(); // List of all permissions on all databases.
            // Start parsing databases and their principals.
            var parsedDatabases = ParseDatabasesAndTheirPrincipals(databases, databaseRoles, databaseUsers, databasePermissions);
            // Get information about instance and it's principals.
            DataRow instanceDetails = collector.GetInstanceInfo().Rows[0];
            DataTable instanceRoles = collector.GetInstanceRoles();
            DataTable instanceLogins = collector.GetInstanceLogins();
            DataTable instancePermissions = collector.GetInstancePermissions();
            // Start parsing of instance principals
            InstanceInfo result = ParseInstancePrincipals(instance.Id, instanceDetails, instanceRoles, instanceLogins, instancePermissions);
            result.Databases.AddRange(parsedDatabases);
            return result;
        }

        public async Task<InstanceInfo> UpdateAsync(Instance instance, IInstanceDataCollector collector)
        {
            // Get information about databases and their principals (inluding permissions).
            // Stars first because it has more data and more time to process it.
            DataTable databases = await collector.GetDatabasesAsync(); // List of all databases on the instance with some basic information (size, date of creation).
            DataTable databaseRoles = await collector.GetDatabaseRolesAsync(); // List of all roles on all databases.
            DataTable databaseUsers = await collector.GetDatabaseUsersAsync(); // List of all users on all databases.
            DataTable databasePermissions = await collector.GetDatabasePermissionsAsync(); // List of all permissions on all databases.
            // Get information about instance and it's principals.
            var instanceDetailsTask = collector.GetInstanceInfoAsync();
            var instanceRolesTask = collector.GetInstanceRolesAsync();
            var instanceLoginsTask = collector.GetInstanceLoginsAsync();
            var instancePermissionsTask = collector.GetInstancePermissionsAsync();
            // Start parsing databases and their principals.
            var parsedDatabases = ParseDatabasesAndTheirPrincipals(databases, databaseRoles, databaseUsers, databasePermissions);
            // Start parsing of instance principals
            InstanceInfo result = ParseInstancePrincipals(
                instance.Id,
                (await instanceDetailsTask).Rows[0],
                await instanceRolesTask,
                await instanceLoginsTask,
                await instancePermissionsTask);
            result.Databases.AddRange(parsedDatabases);
            return result;
        }

        #region Parsing Instance and it's Principals

        private InstanceInfo ParseInstancePrincipals(int nativeInstanceId, DataRow instanceDetails, DataTable instanceRoles, 
            DataTable instanceLogins, DataTable instancePermissions)
        {
            // Parse instance itself.
            InstanceInfo result = ParseInstanceInfo(new InstanceInfo(nativeInstanceId), instanceDetails);
            result.Roles = ParseInstanceRoles(instanceRoles);
            result.Logins = ParseLogins(instanceLogins);
            result.Permissions = ParseInstancePermissions(instancePermissions);
            return result;
        }

        private InstanceInfo ParseInstanceInfo(InstanceInfo instanceInfo, DataRow instanceDetails)
        {
            instanceInfo.Memory = Convert.ToInt32(instanceDetails[0]);
            instanceInfo.CpuCount = Convert.ToByte(instanceDetails[1]);
            instanceInfo.OsVersion = instanceDetails[2].ToString();
            int[] instanceVersion = instanceDetails[3].ToString().Split('.').Select(num => Convert.ToInt32(num)).ToArray();
            instanceInfo.Major = instanceVersion.Length > 0 ? instanceVersion[0] : 0;
            instanceInfo.Minor = instanceVersion.Length >= 1 ? instanceVersion[1] : 0;
            instanceInfo.Build = instanceVersion.Length >= 2 ? instanceVersion[2] : 0;
            instanceInfo.Revision = instanceVersion.Length >= 3 ? instanceVersion[3] : 0;
            return instanceInfo;
        }

        private List<InstRoleInfo> ParseInstanceRoles(DataTable roles)
        {
            // Create list of instance roles with list of IDs of associated with them logins.
            return (from row in roles.AsEnumerable()
                    group row by new { Id = Convert.ToInt32(row[0]), Name = row[1].ToString(), Type = row[2].ToString() } into role
                    select new InstRoleInfo()
                    {
                        NativeId = role.Key.Id,
                        Entity = new InstRole() { Name = role.Key.Name, Type = role.Key.Type },
                        AssociatedIds = role.Where(r => !r.IsNull(3)).Select(r => Convert.ToInt32(r[3])).ToList()
                    }).ToList();
        }

        private List<InstLoginInfo> ParseLogins(DataTable logins)
        {
            // Create list of instance logins with their NativeId (as it saved in database).
            return (from row in logins.AsEnumerable()
                    select new InstLoginInfo()
                    {
                        NativeId = Convert.ToInt32(row[0]),
                        Entity = new InstLogin() { Name = row[1].ToString(), Type = row[2].ToString(), IsDisabled = row[3].ToString() == "1" ? true : false }
                    }).ToList();
        }

        private List<PermissionInfo> ParseInstancePermissions(DataTable permissions)
        {
            // Create new permission objects with list of IDs of associated with them principals.
            return (from row in permissions.AsEnumerable()
                    group row by new { Name = row[0].ToString(), State = row[1].ToString() }
                    into perm
                    select new PermissionInfo()
                    {
                        State = perm.Key.State,
                        Name = perm.Key.Name,
                        AssociatedIds = perm.Select(p => Convert.ToInt32(p[2])).ToList()
                    }).ToList();
        }

        #endregion

        #region Parsing Databases and their Principals

        private List<DatabaseInfo> ParseDatabasesAndTheirPrincipals(DataTable databases, DataTable databaseRoles, DataTable databaseUsers, DataTable databasePermissions)
        {
            var groupedRoles = from row in databaseRoles.AsEnumerable() group row by Convert.ToInt32(row[0]) into g select g;
            var groupedUsers = from row in databaseUsers.AsEnumerable() group row by Convert.ToInt32(row[0]) into g select g;
            var groupedPermissions = from row in databasePermissions.AsEnumerable() group row by Convert.ToInt32(row[0]) into g select g;
            return (from db in ParseDatabases(databases)
                    join roles in groupedRoles on db.NativeId equals roles.Key
                    join users in groupedUsers on db.NativeId equals users.Key
                    join permissions in groupedPermissions on db.NativeId equals permissions.Key
                    select db.ParseRoles(roles).ParseUsers(users).ParsePermissions(permissions)).ToList();
        }

        private List<DatabaseInfo> ParseDatabases(DataTable databases)
        {
            return (from row in databases.AsEnumerable()
                    select new DatabaseInfo()
                    {
                        NativeId = Convert.ToInt32(row[0]),
                        Entity = new Database()
                        {
                            Name = row[1].ToString(),
                            CreateDate = Convert.ToDateTime(row[2]),
                            Size = Convert.ToDecimal(row[3])
                        }
                    }).ToList();
        }

        #endregion

        private string BuildConnectionString(Instance instance)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = instance.DataSource;
            if (instance.Authentication == AuthenticationType.Sql) //!changed  if (instance.Authentication == AuthenticationType.Windows) to AuthenticationType.Sql
            {
                builder.UserID = instance.Login;
                builder.Password = instance.Password;
            }
            else
                builder.IntegratedSecurity = true;
            return builder.ConnectionString;
        }
    }

    internal static class ParseEtensions
    {
        public static DatabaseInfo ParseRoles(this DatabaseInfo database, IEnumerable<DataRow> roles)
        {
            database.Roles = (from row in roles
                              group row by new { Id = Convert.ToInt32(row[1]), Name = row[2].ToString() } into g
                              select new DbRoleInfo()
                              {
                                  NativeId = g.Key.Id,
                                  AssociatedIds = g.Where(r => !r.IsNull(4)).Select(r => Convert.ToInt32(r[4])).ToList(),
                                  Entity = new DbRole()
                                  {
                                      Database = database.Entity,
                                      Name = g.Key.Name,
                                  }
                              }).ToList();
            database.Roles.ForEach(role => database.Entity.Roles.Add(role.Entity));
            return database;
        }

        public static DatabaseInfo ParseUsers(this DatabaseInfo database, IEnumerable<DataRow> users)
        {
            database.Users = (from row in users
                              select new DbUserInfo()
                              {
                                  NativeId = Convert.ToInt32(row[1]),
                                  Entity = new DbUser()
                                  {
                                      Database = database.Entity,
                                      Name = row[2].ToString(),
                                      Type = row[3].ToString()
                                  }
                              }).ToList();
            database.Users.ForEach(user => database.Entity.Users.Add(user.Entity));
            return database;
        }

        public static DatabaseInfo ParsePermissions(this DatabaseInfo database, IEnumerable<DataRow> permissions)
        {
            database.Permissions = (from row in permissions
                                    group row by new { Name = row[1].ToString(), State = row[2].ToString() } into perm
                                    select new PermissionInfo()
                                    {
                                        State = perm.Key.State,
                                        Name = perm.Key.Name,
                                        AssociatedIds = perm.Select(p => Convert.ToInt32(p[3])).ToList()
                                    }).ToList();
            return database;
        }
    }
}
