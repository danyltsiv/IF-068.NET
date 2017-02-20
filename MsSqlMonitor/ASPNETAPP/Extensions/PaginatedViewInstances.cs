using ASPNETAPP.ViewModels;
using DALLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETAPP.Extensions
{
    public class PaginatedViewInstances
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int PagesCount { get; set; }
        public IEnumerable<InstanceViewModel> List { get; set; }

        public IEnumerable<String> Versions { get; set; }

        public String Error { get; set; }

        public PaginatedViewInstances(PaginatedInstances i)
        {
            PageSize = i.PageSize;
            Page = i.Page;
            PagesCount = i.PageCount;
            List = i.List.Select(k => k.ToViewModel());
            Versions = i.Versions;
            Error = i.Error;
        }
    }
}