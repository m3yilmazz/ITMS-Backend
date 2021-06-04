using System;

namespace ITMSWebAPI.DTO.Response.Debit
{
    public class DebitGetAllResponse
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string Assigner { get; set; }
        public string User { get; set; }
        public string AssetType { get; set; }
        public string AssetName { get; set; }
        public string AssetDescription { get; set; }
        public string Cause { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreatedDate { get; set; }
        public string EditedDate { get; set; }
        public bool isDelivered { get; set; }
    }
}
