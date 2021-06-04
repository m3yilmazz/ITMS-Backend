using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITMSWebAPI.DTO.Request.User
{
    public class UserUpdateRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
