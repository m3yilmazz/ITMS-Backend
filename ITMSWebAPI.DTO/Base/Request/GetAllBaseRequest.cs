using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITMSWebAPI.DTO.Base.Request
{
    public class GetAllBaseRequest
    {
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
