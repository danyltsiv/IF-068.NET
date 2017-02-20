using DALLib.Contracts;
using DALLib.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.Contracts
{
    public interface IConnectionManager
    {
        SqlConnection Connection { get;  }

        Task<SqlConnection> OpenConnection(int instanceID, IUnitOfWork unitOfWork);

        void CloseConnection();
    }
}
