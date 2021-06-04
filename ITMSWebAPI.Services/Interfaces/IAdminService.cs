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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITMSWebAPI.Services.Interfaces
{
    public interface IAdminService
    {
        AdminLoginResponse AdminLogin(AdminLoginRequest request);
        AdminLogoffResponse AdminLogoff();
        UserAddResponse AddUser(UserAddRequest request);
        AssetAddResponse AddAsset(AssetAddRequest request);
        DebitAddResponse AddDebit(DebitAddRequest request, string guid);
        UserRemoveResponse RemoveUser(UserRemoveRequest request);
        AssetRemoveResponse RemoveAsset(AssetRemoveRequest request);
        DebitRemoveResponse RemoveDebit(DebitRemoveRequest request);
        UserUpdateResponse UpdateUser(UserUpdateRequest request);
        AssetUpdateResponse UpdateAsset(AssetUpdateRequest request);
        DebitUpdateResponse UpdateDebit(DebitUpdateRequest request);
        GetAllBaseResponse<UserGetAllResponse> GetAllUsers(UserGetAllRequest request);
        GetAllBaseResponse<AssetGetAllResponse> GetAllAssets(AssetGetAllRequest request);
        GetAllBaseResponse<DebitGetAllResponse> GetAllDebits(DebitGetAllRequest request);
        List<AssetGetAllResponse> FilterAssets(IQueryable<AssetGetAllResponse> queryable, string filterByType, string filterByIsAssigned, string sortByName);
        IQueryable<AssetGetAllResponse> FilterAssetsWithoutPaging(IQueryable<AssetGetAllResponse> queryable, string filterByType, string filterByIsAssigned, string sortByName);
        List<DebitGetAllResponse> FilterDebits(IQueryable<DebitGetAllResponse> queryable, string filterByType, string filterByIsDelivered, string sortByAssetName);
        IQueryable<DebitGetAllResponse> FilterDebitsWithoutPaging(IQueryable<DebitGetAllResponse> queryable, string filterByType, string filterByIsDelivered, string sortByAssetName);
    }
}
