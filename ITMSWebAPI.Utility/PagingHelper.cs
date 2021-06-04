using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITMSWebAPI.Utility
{
    public static class PagingHelper
    {
        const int pageSize = 10;
        public static int PageCounter(int dataCount)
        {
            int pageCount = dataCount / pageSize;
            if (dataCount % pageSize != 0)
                pageCount += 1;
            return pageCount;
        }
    }
}
