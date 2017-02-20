using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib
{
    public enum SQLVersion
    {
        SQL2000,
        SQL2005,
        SQL2008,
        SQL2012,
        SQL2014,
        SQL2016,
        OTHER
    }


    public class SQLVersions
    {


        public static SQLVersion GetSQLVersionFromString(string version)
        {
            if (version == null) return SQLVersion.OTHER;

            if (version.StartsWith("8.")) return SQLVersion.SQL2000;
            if (version.StartsWith("9.")) return SQLVersion.SQL2005;
            if (version.StartsWith("10.")) return SQLVersion.SQL2008;
            if (version.StartsWith("11.")) return SQLVersion.SQL2012;
            if (version.StartsWith("12.")) return SQLVersion.SQL2014;
            if (version.StartsWith("13.")) return SQLVersion.SQL2016;

            return SQLVersion.OTHER;
        }


        public static bool IsVersionInlist(SQLVersion version,string list)
        {
            switch (version)
            {
                case SQLVersion.SQL2000 : 
                               if (list.ElementAt(6) == '1') return true;
                                else return false;

                case SQLVersion.SQL2005:
                    if (list.ElementAt(5) == '1') return true;
                    else return false;

                case SQLVersion.SQL2008:
                    if (list.ElementAt(4) == '1') return true;
                    else return false;

                case SQLVersion.SQL2012:
                    if (list.ElementAt(3) == '1') return true;
                    else return false;

                case SQLVersion.SQL2014:
                    if (list.ElementAt(2) == '1') return true;
                    else return false;

                case SQLVersion.SQL2016:
                    if (list.ElementAt(1) == '1') return true;
                    else return false;

                case SQLVersion.OTHER:
                    if (list.ElementAt(0) == '1') return true;
                    else return false;

                 default :
                     return false;
            }


            return false;
        }

    }
}
