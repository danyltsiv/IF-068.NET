using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.DataProvider
{

    [Serializable]
    public class EntityNotExistException : Exception
    {
        public EntityNotExistException() { }
        public EntityNotExistException(string message) : base(message) { }
        public EntityNotExistException(string message, Exception inner) : base(message, inner) { }
        protected EntityNotExistException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
