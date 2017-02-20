using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAPP.Responses
{
    public interface IListModelResponse<TModel> : IResponse
    {
        int PageSize { get; set; }

        int PageNumber { get; set; }

        int PagesCount { get; set; }

        string NameFilter { get; set; }

        IEnumerable<String> Versions { get; set; }

        IEnumerable<TModel> Model { get; set; }
    }
}
