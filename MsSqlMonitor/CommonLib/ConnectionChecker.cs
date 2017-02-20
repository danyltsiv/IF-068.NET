using DALLib.Impersonation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class ConnectionChecker
    {
        public static async Task<string> Check(string servername, string instanceName, string login,string pswd , bool windowsoauth )
        {
            String connString = BuildConnectionString(servername,instanceName, login,pswd,windowsoauth);

            Impersonation impersonation = null;
            ImpersonationResult impResult = null;

            SqlConnection connection = new SqlConnection(connString);

            if (windowsoauth)
            {
                impersonation = new Impersonation();
                impResult = impersonation.Impersonate(login, null, pswd);

                if (impResult.HasError) return "Windows impersonation error!";          
            }


            try
            {
                
                await connection.OpenAsync().ConfigureAwait(true);
                connection.Close();
            }
            catch (Exception e)
            {
                return "Open connection error!\n"+e.Message;
         
            }
            finally
            {
                if (impResult != null && impersonation != null)
                    if (!impResult.HasError) impersonation.UndoImpersonation(impResult.User);

                
            }


            return "Ok!";
        }


        private static string BuildConnectionString(string servername, string instanceName, string login, string pswd, bool windowsoauth)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = servername + @"\" + instanceName;

            if (!windowsoauth)
            {
                builder.UserID = login;
                builder.Password = pswd;
            }
            else
                builder.IntegratedSecurity = true;
            return builder.ConnectionString;
        }

    }
}
