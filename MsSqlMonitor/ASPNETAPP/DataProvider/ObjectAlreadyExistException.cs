using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETAPP.DataProvider
{

    [Serializable]
    public class ObjectAlreadyExistException : Exception
    {
        public ObjectAlreadyExistException() { }
        public ObjectAlreadyExistException(string message) : base(message) { }
        public ObjectAlreadyExistException(string message, Exception inner) : base(message, inner) { }
        protected ObjectAlreadyExistException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}