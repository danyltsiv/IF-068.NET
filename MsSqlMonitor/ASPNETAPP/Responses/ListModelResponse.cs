using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.Responses
{
    public class ListModelResponse<TModel> : IListModelResponse<TModel>
    {
        public bool DidError { get; set; }

        public string ErrorMessage { get; set; }

        public string Message { get; set; }

        public IEnumerable<TModel> Model { get; set; }

        public IEnumerable<String> Versions { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int PagesCount { get; set; }

        public string NameFilter { get; set; }
    }
}
