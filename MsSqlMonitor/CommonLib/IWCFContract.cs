using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    [ServiceContract(Namespace = "WCFCollectorNamespace")]
    public interface IWCFContract
    {
        [OperationContract]
        void RefreshInstance(int id);
    }
}
