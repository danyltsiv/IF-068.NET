using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib
{
    public class PaginatedInstances
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<Instance> List { get; set; }

        public IEnumerable<String> Versions { get; set; }

        public String Error { get; set; }
    }
}
