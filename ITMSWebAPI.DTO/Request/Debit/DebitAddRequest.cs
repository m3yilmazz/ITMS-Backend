namespace ITMSWebAPI.DTO.Request.Debit
{
    public class DebitAddRequest
    {
        public string UserEmail { get; set; }
        public int AssetId { get; set; }
        public long EndDate { get; set; }
        public string Cause { get; set; }
    }
}
