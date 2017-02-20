using SQLInfoCollectionService.Contracts;
using System;
using System.Data.SqlClient;
using DALLib.Models;
using DALLib.Impersonation;
using System.Threading.Tasks;
using DALLib.Contracts;
using CommonLib;
using SQLInfoCollectorService.Security;

namespace SQLInfoCollectionService.Entities
{
    public class ConnectionManager : IConnectionManager,IDisposable
    {
        private SqlConnection connection = null;
        private ISLogger logger;
        private IEncryptionManager encryptionManager;

        public SqlConnection Connection
        {
            get
            {
                return connection;
            }

        
        }



        public ConnectionManager(ISLogger logger, IEncryptionManager encryptionManager)
        {
            this.logger = logger;
            this.encryptionManager = encryptionManager;
        }

        private string BuildConnectionString(Instance instance)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = instance.DataSource;
            if (instance.Authentication == AuthenticationType.Sql)
            {
                builder.UserID = instance.Login;
                //builder.Password = instance.Password;
                builder.Password = encryptionManager.Decrypt(instance.EncryptionKey, instance.Password);
            }
            else
                builder.IntegratedSecurity = true;
            return builder.ConnectionString;
        }



        public void CloseConnection()
        {
            if (connection != null)
            {
                connection.Close();
                connection = null;
            }
        }

        public async Task<SqlConnection> OpenConnection(Instance instance)
        {
            if (instance == null)
            {
                logger.Error("ConnectionManager.OpenConnection: get null instance" );
                return null;
            }

        Impersonation impersonation = null;
        ImpersonationResult impResult = null;

        connection = new SqlConnection(BuildConnectionString(instance));


            if (instance.Authentication == AuthenticationType.Windows)
            {
                impersonation = new Impersonation();
                impResult = impersonation.Impersonate(instance.Login, null, instance.Password);//тут

                if (impResult.HasError) logger.Error("Imperosnation error: ConnectionManager.OpenConnection: Instance id =" + instance.Id );
                else logger.Debug("Impersonation oK instance id =" + instance.Id);

                return null;
            }


            try
            {
                //ConfigureAwait(true); to return to the same context to do undo imersonation for the same thread
                await connection.OpenAsync().ConfigureAwait(true);
            }
            catch (Exception e)
            {
                logger.Error("ConnectionManager.OpenConnection: Error open connection  instance=" + instance.InstanceName, e);
                return null;
            }
            finally
            {
                if (impResult != null && impersonation != null)
                    if (!impResult.HasError) impersonation.UndoImpersonation(impResult.User);
            }

            return connection;
        }


        public async Task<SqlConnection> OpenConnection(int instanceID,IUnitOfWork unitOfWork)
        {
            Instance instance = unitOfWork.Instances.Get(instanceID);

            return await OpenConnection(instance);

        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
