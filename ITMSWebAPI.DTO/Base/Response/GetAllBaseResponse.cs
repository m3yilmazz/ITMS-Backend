using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITMSWebAPI.DTO.Base.Response
{
    public class GetAllBaseResponse<T> : BaseResponse where T : class
    {
        public List<T> RecordList { get; set; }
        public int DataCount { get; set; }
        public int PageCount { get; set; }
    }
}
