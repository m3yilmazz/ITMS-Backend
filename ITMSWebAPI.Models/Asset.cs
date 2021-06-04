using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITMSWebAPI.Models
{
    public class Asset
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long AddedDate { get; set; }
        public bool isAssigned { get; set; }
        public long ExpiryDate { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public string PersonEmail { get; set; }
    }
}
