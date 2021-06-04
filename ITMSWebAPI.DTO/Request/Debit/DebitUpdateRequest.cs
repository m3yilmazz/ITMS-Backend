namespace ITMSWebAPI.DTO.Request.Debit
{
    public class DebitUpdateRequest
    {
        public int DebitId { get; set; }
        public int AssetId { get; set; }
        public long EndDate { get; set; }
        public bool isDelivered { get; set; }
    }
}
