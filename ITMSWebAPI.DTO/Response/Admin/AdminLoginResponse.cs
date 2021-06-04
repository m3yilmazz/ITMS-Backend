using ITMSWebAPI.DTO.Base.Response;

namespace ITMSWebAPI.DTO.Response.Admin
{
    public class AdminLoginResponse : BaseResponse
    {
        public AdminModel AdminModel { get; set; }
    }

    public class AdminModel
    {
        public AdminInfo AdminInfo { get; set; }
    }
}
