using System;

namespace ITMSWebAPI.DTO.Request.Asset
{
    public class AssetAddRequest
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ExpiryDate { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public string PersonEmail { get; set; }
    }
}
