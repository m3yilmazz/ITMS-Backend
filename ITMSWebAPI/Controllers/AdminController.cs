using ITMSWebAPI.DTO.Base.Response;
using ITMSWebAPI.DTO.Request.Admin;
using ITMSWebAPI.DTO.Request.Asset;
using ITMSWebAPI.DTO.Request.Debit;
using ITMSWebAPI.DTO.Request.User;
using ITMSWebAPI.DTO.Response.Admin;
using ITMSWebAPI.DTO.Response.Asset;
using ITMSWebAPI.DTO.Response.Debit;
using ITMSWebAPI.DTO.Response.User;
using ITMSWebAPI.Models;
using ITMSWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ITMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ISessionInfoService _sessionInfoService;
        public AdminController(IAdminService adminService, ISessionInfoService sessionInfoService)
        {
            _adminService = adminService;
            _sessionInfoService = sessionInfoService;
        }

        [HttpPost("AdminLogin")]
        public AdminLoginResponse AdminLogin(AdminLoginRequest request)
        {
            var response = _adminService.AdminLogin(request);
            if (!response.Success)
            {
                return new AdminLoginResponse() 
                { 
                    Success = false 
                };
            }
            var guid = _sessionInfoService.AssignSessionInfo(response.AdminModel.AdminInfo.AdminId);
            HttpContext.Response.Cookies.Append("sessionid", guid);

            return response;
        }

        [HttpGet("AdminLogoff")]
        public AdminLogoffResponse AdminLogoff()
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            
            if (string.IsNullOrEmpty(guid))
            {
                return new AdminLogoffResponse
                {
                    Success = false
                };
            }

            if (!_sessionInfoService.RemoveSessionInfo(guid))
            {
                return new AdminLogoffResponse
                {
                    Success = false
                };
            }

            return _adminService.AdminLogoff();
        }

        [HttpPost("AddUser")]
        public UserAddResponse AddUser(UserAddRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new UserAddResponse()
                { 
                    Success = false 
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new UserAddResponse()
                {
                    Success = false
                };
            }

            return _adminService.AddUser(request);
        }

        [HttpPost("AddAsset")]
        public AssetAddResponse AddAsset(AssetAddRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new AssetAddResponse()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new AssetAddResponse()
                {
                    Success = false
                };
            }

            return _adminService.AddAsset(request);
        }

        [HttpPost("AddDebit")]
        public DebitAddResponse AddDebit(DebitAddRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new DebitAddResponse()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new DebitAddResponse()
                {
                    Success = false
                };
            }

            return _adminService.AddDebit(request, guid);
        }

        [HttpPost("RemoveUser")]
        public UserRemoveResponse RemoveUser(UserRemoveRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new UserRemoveResponse()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new UserRemoveResponse()
                {
                    Success = false
                };
            }

            return _adminService.RemoveUser(request);
        }

        [HttpPost("RemoveAsset")]
        public AssetRemoveResponse RemoveAsset(AssetRemoveRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new AssetRemoveResponse()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new AssetRemoveResponse()
                {
                    Success = false
                };
            }

            return _adminService.RemoveAsset(request);
        }
        
        [HttpPost("RemoveDebit")]
        public DebitRemoveResponse RemoveDebit(DebitRemoveRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new DebitRemoveResponse()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new DebitRemoveResponse()
                {
                    Success = false
                };
            }

            return _adminService.RemoveDebit(request);
        }
        
        [HttpPost("UpdateUser")]
        public UserUpdateResponse UpdateUser(UserUpdateRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new UserUpdateResponse()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new UserUpdateResponse()
                {
                    Success = false
                };
            }
            return _adminService.UpdateUser(request);
        }

        [HttpPost("UpdateAsset")]
        public AssetUpdateResponse UpdateAsset(AssetUpdateRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new AssetUpdateResponse()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new AssetUpdateResponse()
                {
                    Success = false
                };
            }
            return _adminService.UpdateAsset(request);
        }

        [HttpPost("UpdateDebit")]
        public DebitUpdateResponse UpdateDebit(DebitUpdateRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new DebitUpdateResponse()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new DebitUpdateResponse()
                {
                    Success = false
                };
            }
            return _adminService.UpdateDebit(request);
        }

        [HttpPost("GetAllUsers")]
        public GetAllBaseResponse<UserGetAllResponse> GetAllUsers(UserGetAllRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new GetAllBaseResponse<UserGetAllResponse>()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new GetAllBaseResponse<UserGetAllResponse>()
                {
                    Success = false
                };
            }

            return _adminService.GetAllUsers(request);
        }

        [HttpPost("GetAllAssets")]
        public GetAllBaseResponse<AssetGetAllResponse> GetAllAssets(AssetGetAllRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new GetAllBaseResponse<AssetGetAllResponse>()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new GetAllBaseResponse<AssetGetAllResponse>()
                {
                    Success = false
                };
            }

            return _adminService.GetAllAssets(request);
        }

        [HttpPost("GetAllDebits")]
        public GetAllBaseResponse<DebitGetAllResponse> GetAllDebits(DebitGetAllRequest request)
        {
            HttpContext.Request.Cookies.TryGetValue("sessionid", out string guid);
            if (string.IsNullOrEmpty(guid))
            {
                return new GetAllBaseResponse<DebitGetAllResponse>()
                {
                    Success = false
                };
            }

            bool isSessionExist = _sessionInfoService.SessionExistanceChecker(guid);
            if (!isSessionExist)
            {
                return new GetAllBaseResponse<DebitGetAllResponse>()
                {
                    Success = false
                };
            }

            return _adminService.GetAllDebits(request);
        }
    }
}
