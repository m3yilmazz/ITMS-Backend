using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITMSWebAPI.Models
{
    public class Debit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("Asset")]
        public int AssetId { get; set; }

        [ForeignKey("Admin")]
        public int AdminId { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public long CreatedDate { get; set; }
        public long EditedDate { get; set; }
        public string Cause { get; set; }
        public bool isDelivered { get; set; }

        public virtual Asset Asset { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual User User { get; set; }
    }
}
