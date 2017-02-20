﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETAPP.DataProvider
{

    [Serializable]
    public class InvalidModelException : Exception
    {
        public InvalidModelException() { }
        public InvalidModelException(string message) : base(message) { }
        public InvalidModelException(string message, Exception inner) : base(message, inner) { }
        protected InvalidModelException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}