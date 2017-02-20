using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALLib.Models;
using System.Data.SqlClient;

namespace SQLInfoCollectionService.DataUpdating.HelpClasses
{
    static class Extensions
    {
        public static string BuildConnectionString(this Instance instance)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = instance.DataSource;
            if (instance.Authentication == AuthenticationType.Sql)
            {
                builder.UserID = instance.Login;
                builder.Password = instance.Password;
            }
            else
                builder.IntegratedSecurity = true;
            return builder.ConnectionString;
        }
    }
}
