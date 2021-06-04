using ITMSWebAPI.DTO.Base.Request;

namespace ITMSWebAPI.DTO.Request.Asset
{
    public class AssetGetAllRequest : GetAllBaseRequest
    {
        public string FilterByType { get; set; }
        public string FilterByIsAssigned { get; set; }
        public string SortByName { get; set; }
    }
}
