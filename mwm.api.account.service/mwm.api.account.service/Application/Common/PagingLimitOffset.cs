using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application
{
    public static class PagingLimitOffset
    {
        /// <summary>
        /// select * from table limit numberperpage offset (pagenumber-1)*numberperpage
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageRows"></param>
        /// <returns></returns>
        public static int GetOffset(int pageIndex, int pageRows)
        {
            if (pageIndex < 1) return 0;
            return (pageIndex-1)* pageRows;
        }
    }
}
