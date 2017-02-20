using System.Collections.Generic;
using SQLInfoCollectionService.Entities;
using SQLInfoCollectionService.Scheduler;
using System.Data;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.Contracts
{
    public interface IInstanceDataCollector
    {

        /// <summary>
        /// TODO: We need to decide what exactly it should return.
        /// Now it should have columns
        /// </summary>
        Task<DataTable> GetInstanceInfoAsync();
        DataTable GetInstanceInfo();

        /// <summary>
        /// Returns roles from the instance.
        /// Columns sequence: principal_id, name, type_desc, princpal_id of assigned users
        /// </summary>
        Task<DataTable> GetInstanceRolesAsync();
        DataTable GetInstanceRoles();

        /// <summary>
        /// Returns all logins on the instance
        /// Columns sequence: database_id, principal_id, name, type_desc, is_disabled (as numeric value 1 or 0)
        /// </summary>
        Task<DataTable> GetInstanceLoginsAsync();
        DataTable GetInstanceLogins();

        /// <summary>
        /// Returns all permissions from the instance.
        /// Columns sequence: permission_name, state_desc, grantee_principal_id
        /// </summary>
        Task<DataTable> GetInstancePermissionsAsync();
        DataTable GetInstancePermissions();

        /// <summary>
        /// Returns all databases from the instance.
        /// Columns sequence: database_id, name, Size in MB.
        /// </summary>
        Task<DataTable> GetDatabasesAsync();
        DataTable GetDatabases();

        /// <summary>
        /// Returns all roles from all databases of the instance.
        /// Columns sequence: database_id, principal_id, name, type_desc, princpal_id of assigned users
        /// </summary>
        Task<DataTable> GetDatabaseRolesAsync();
        DataTable GetDatabaseRoles();

        /// <summary>
        /// Returns all users from all databases of the instance.
        /// Columns sequence: database_id, principal_id, name, type_desc
        /// </summary>
        Task<DataTable> GetDatabaseUsersAsync();
        DataTable GetDatabaseUsers();

        /// <summary>
        /// Returns all permissions from all databases of the instance.
        /// Columns sequence: database_id, permission_name, state_desc, grantee_principal_id
        /// </summary>
        Task<DataTable> GetDatabasePermissionsAsync();
        DataTable GetDatabasePermissions();
    }
}
