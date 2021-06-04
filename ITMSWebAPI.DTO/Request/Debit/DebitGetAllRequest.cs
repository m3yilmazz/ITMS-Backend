using ITMSWebAPI.DTO.Base.Request;

namespace ITMSWebAPI.DTO.Request.Debit
{
    public class DebitGetAllRequest : GetAllBaseRequest
    {
        public string FilterByType { get; set; }
        public string FilterByIsDelivered { get; set; }
        public string SortByAssetName { get; set; }
    }
}
