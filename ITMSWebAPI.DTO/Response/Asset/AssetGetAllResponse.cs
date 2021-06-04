using ITMSWebAPI.DTO.Base.Response;
using System;

namespace ITMSWebAPI.DTO.Response.Asset
{
    public class AssetGetAllResponse
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AddedDate { get; set; }
        public bool isAssigned { get; set; }
        public string ExpiryDate { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public string PersonEmail { get; set; }
    }
}
