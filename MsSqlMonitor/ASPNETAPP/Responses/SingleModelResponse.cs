using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.Responses
{
    public class SingleModelResponse<TModel> : ISingleModelResponse<TModel>
    {
        public bool DidError { get; set; }

        public string ErrorMessage { get; set; }

        public string Message { get; set; }

        public TModel Model { get; set; }
    }
}
