namespace ITMSWebAPI.DTO.Request.Asset
{
    public class AssetUpdateRequest
    {
        public int AssetId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ExpiryDate { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public string PersonEmail { get; set; }
        public bool isAssigned { get; set; }
    }
}
