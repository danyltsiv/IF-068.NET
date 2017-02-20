using System;
using System.Collections.Generic;
using System.Linq;
using SQLInfoCollectionService.Entities;
using System.Data;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Scheduler;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CommonLib;

namespace SQLInfoCollectionService.Collectors
{
    public class InstanceDataCollector : IInstanceDataCollector
    {
        private IConnectionManager connManager;
        private SqlCommand command;
        private ISLogger logger;
        private IResourceManager resourceManager;

        public InstanceDataCollector(IConnectionManager connManager, IResourceManager resourceManager, ISLogger logger)
        {
            this.connManager = connManager;
            command = new SqlCommand();
            command.Connection = connManager.Connection;
            this.logger = logger;
            this.resourceManager = resourceManager;
        }

        //synchronus methods
        public DataTable GetInstanceInfo()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstanceInfo");
            string script = resourceManager.GetInstanceDetailsScript(connManager.Connection.ServerVersion);
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetInstanceRoles()
        {
           // logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstanceRoles");
            string script = resourceManager.GetInstanceRolesScript(connManager.Connection.ServerVersion);
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetInstanceLogins()
        {
           // logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstanceLogins");
            string script = resourceManager.GetInstanceLoginsScript(connManager.Connection.ServerVersion);
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetInstancePermissions()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstancePermissions");
            string script = resourceManager.GetInstancePermissionsScript(connManager.Connection.ServerVersion);
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetDatabases()
        {
           // logger.Debug("at SQLInfoCollectionService.Collectors.GetDatabases.CollectDatabases");
            string script = resourceManager.GetDatabasesScript(connManager.Connection.ServerVersion);
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetDatabaseRoles()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetDatabaseRoles");
            string script = resourceManager.GetDbRolesScript(connManager.Connection.ServerVersion);
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetDatabaseUsers()
        {
           // logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetDatabaseUsers");
            string script = resourceManager.GetDbUsersScript(connManager.Connection.ServerVersion);
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetDatabasePermissions()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetDatabasePermissions");
            string script = resourceManager.GetDbPermissionsScript(connManager.Connection.ServerVersion);
            this.command.CommandText = script;

            return FillTable();
        }

        private DataTable FillTable()
        {
            using (IDataReader reader = command.ExecuteReader())
            {
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        //asynchronus methods
        public async Task<DataTable> GetInstanceInfoAsync()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstanceInfo");
            string script = resourceManager.GetInstanceDetailsScript(connManager.Connection.ServerVersion);
            SqlCommand command = new SqlCommand(script);
            command.Connection = connManager.Connection;

            return await FillTableAsync(command).ConfigureAwait(false);
        }

        public async Task<DataTable> GetInstanceRolesAsync()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstanceRoles");
            string script = resourceManager.GetInstanceRolesScript(connManager.Connection.ServerVersion);
            SqlCommand command = new SqlCommand(script);
            command.Connection = connManager.Connection;

            return await FillTableAsync(command).ConfigureAwait(false);
        }

        public async Task<DataTable> GetInstanceLoginsAsync()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstanceLogins");
            string script = resourceManager.GetInstanceLoginsScript(connManager.Connection.ServerVersion);
            SqlCommand command = new SqlCommand(script);
            command.Connection = connManager.Connection;

            return await FillTableAsync(command).ConfigureAwait(false);
        }

        public async Task<DataTable> GetInstancePermissionsAsync()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstancePermissions");
            string script = resourceManager.GetInstancePermissionsScript(connManager.Connection.ServerVersion);
            SqlCommand command = new SqlCommand(script);
            command.Connection = connManager.Connection;

            return await FillTableAsync(command).ConfigureAwait(false);
        }

        public async Task<DataTable> GetDatabasesAsync()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.GetDatabases.CollectDatabases");
            string script = resourceManager.GetDatabasesScript(connManager.Connection.ServerVersion);
            SqlCommand command = new SqlCommand(script);
            command.Connection = connManager.Connection;

            return await FillTableAsync(command).ConfigureAwait(false);
        }

        public async Task<DataTable> GetDatabaseRolesAsync()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetDatabaseRoles");
            string script = resourceManager.GetDbRolesScript(connManager.Connection.ServerVersion);
            SqlCommand command = new SqlCommand(script);
            command.Connection = connManager.Connection;

            return await FillTableAsync(command).ConfigureAwait(false);
        }

        public async Task<DataTable> GetDatabaseUsersAsync()
        {
            //logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetDatabaseUsers");
            string script = resourceManager.GetDbUsersScript(connManager.Connection.ServerVersion);
            SqlCommand command = new SqlCommand(script);
            command.Connection = connManager.Connection;

            return await FillTableAsync(command).ConfigureAwait(false);
        }

        public async Task<DataTable> GetDatabasePermissionsAsync()
        {
           // logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetDatabasePermissions");
            string script = resourceManager.GetDbPermissionsScript(connManager.Connection.ServerVersion);
            SqlCommand command = new SqlCommand(script);
            command.Connection = connManager.Connection;

            return await FillTableAsync(command).ConfigureAwait(false);
        }

        private async Task<DataTable> FillTableAsync(SqlCommand command)
        {
            using (IDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
            {
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }
    }
}
