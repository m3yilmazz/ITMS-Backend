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
using ITMSWebAPI.Models.Context;
using ITMSWebAPI.Services.Interfaces;
using ITMSWebAPI.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITMSWebAPI.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationContext _applicationContext;

        public AdminService(ApplicationContext applicationContext) : base()
        {
            _applicationContext = applicationContext;
        }

        public AdminLoginResponse AdminLogin(AdminLoginRequest request)
        {
            if(request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return new AdminLoginResponse()
                {
                    Success = false
                };
            }

            var queriedAdmin = _applicationContext.Admins.FirstOrDefault(q => q.Email == request.Email && q.Password == request.Password);

            if(queriedAdmin == null)
            {
                return new AdminLoginResponse()
                {
                    Success = false
                };
            }

            AdminLoginResponse adminLoginResponse = new AdminLoginResponse { Success = true };

            AdminInfo adminInfo = new AdminInfo()
            {
                AdminId = queriedAdmin.Id,
                Name = queriedAdmin.Name,
                Surname = queriedAdmin.Surname,
                Email = queriedAdmin.Email,
                TelephoneNumber = queriedAdmin.TelephoneNumber
            };

            adminLoginResponse.AdminModel = new AdminModel()
            {
                AdminInfo = adminInfo
            };

            return adminLoginResponse;
        }

        public AdminLogoffResponse AdminLogoff()
        {
            return new AdminLogoffResponse()
            { 
                Success = true 
            };
        }

        public UserAddResponse AddUser(UserAddRequest request)
        {
            if(request == null)
            {
                return new UserAddResponse()
                {
                    Success = false
                };
            }

            var userExist = _applicationContext.Users.Any(q => q.Email == request.Email || q.TelephoneNumber == request.TelephoneNumber);

            if (userExist)
            {
                return new UserAddResponse()
                {
                    Success = false
                };
            }

            using (var transaction = _applicationContext.Database.BeginTransaction())
            {
                try
                {

                    _applicationContext.Users.Add(new User()
                    {
                        Name = request.Name,
                        Surname = request.Surname,
                        Email = request.Email,
                        TelephoneNumber = request.TelephoneNumber
                    });
                    _applicationContext.SaveChanges();

                    transaction.Commit();
                    return new UserAddResponse
                    {
                        Success = true
                    };
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Console.Write(exception);
                    return new UserAddResponse
                    {
                        Success = false
                    };
                }
            }
        }

        public AssetAddResponse AddAsset(AssetAddRequest request)
        {
            if (request == null)
            {
                return new AssetAddResponse()
                {
                    Success = false
                };
            }
            if (string.Compare(request.Type.ToLower(), "dijital") == 0)
            {
                var digitalAssetExist = _applicationContext.Assets.Any(q => q.Description == request.Description);

                if (digitalAssetExist)
                {
                    return new AssetAddResponse()
                    {
                        Success = false
                    };
                }
            }

            if(string.Compare(request.Type.ToLower(), "insan kaynağı") == 0)
            {
                var personAssetExist = _applicationContext.Assets.Any(q => q.PersonEmail == request.PersonEmail);

                if (personAssetExist)
                {
                    return new AssetAddResponse()
                    {
                        Success = false
                    };
                }
            }

            using (var transaction = _applicationContext.Database.BeginTransaction())
            {
                try
                {
                    _applicationContext.Assets.Add(new Asset()
                    {
                        Type = request.Type,
                        Name = request.Name,
                        Description = request.Description,
                        AddedDate = DateTimeExtension.ToUnixTime(DateTime.Now),
                        ExpiryDate = request.ExpiryDate,
                        PersonName = request.PersonName,
                        PersonSurname = request.PersonSurname,
                        PersonEmail = request.PersonEmail,
                        isAssigned = false
                    });
                    _applicationContext.SaveChanges();

                    transaction.Commit();
                    return new AssetAddResponse
                    {
                        Success = true
                    };
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Console.Write(exception);
                    return new AssetAddResponse
                    {
                        Success = false
                    };
                }
            }
        }

        public DebitAddResponse AddDebit(DebitAddRequest request, string guid)
        {
            if (request == null)
            {
                return new DebitAddResponse()
                {
                    Success = false
                };
            }

            var relationalAsset = _applicationContext.Assets.FirstOrDefault(q => q.Id == request.AssetId);
            
            if(relationalAsset == null)
            {
                return new DebitAddResponse()
                {
                    Success = false
                };
            }

            if(relationalAsset.isAssigned == true)
            {
                return new DebitAddResponse()
                {
                    Success = false
                };
            }
            
            var relationalUser = _applicationContext.Users.FirstOrDefault(q => q.Email == request.UserEmail);
            
            if (relationalUser == null)
            {
                return new DebitAddResponse()
                {
                    Success = false
                };
            }

            using (var transaction = _applicationContext.Database.BeginTransaction())
            {
                try
                {
                    Debit debitObject = new Debit()
                    {
                        AssetId = relationalAsset.Id,
                        AdminId = _applicationContext.SessionInfos.FirstOrDefault(q => q.SessionGuid == guid).AdminId,
                        UserId = relationalUser.Id,
                        StartDate = DateTimeExtension.ToUnixTime(DateTime.Now),
                        EndDate = request.EndDate,
                        CreatedDate = DateTimeExtension.ToUnixTime(DateTime.Now),
                        Cause = request.Cause,
                        isDelivered = false
                    };

                    _applicationContext.Debits.Add(debitObject);

                    _applicationContext.Assets.FirstOrDefault(q => q.Id == debitObject.AssetId).isAssigned = true;

                    _applicationContext.SaveChanges();

                    transaction.Commit();
                    return new DebitAddResponse
                    {
                        Success = true
                    };
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Console.Write(exception);
                    return new DebitAddResponse
                    {
                        Success = false
                    };
                }
            }
        }

        public UserRemoveResponse RemoveUser(UserRemoveRequest request)
        {
            if (request == null || request.UserId == default(int))
            {
                return new UserRemoveResponse()
                {
                    Success = false
                };
            }

            var isAtLeastOneRelatedDebitWithUser = _applicationContext.Debits.Any(q => q.UserId == request.UserId && q.isDelivered == false);
            if (isAtLeastOneRelatedDebitWithUser)
            {
                return new UserRemoveResponse()
                {
                    Success = false
                };
            }

            try
            {
                _applicationContext.Remove(_applicationContext.Users.Single(q => q.Id == request.UserId));
                _applicationContext.SaveChanges();
                return new UserRemoveResponse
                {
                    Success = true
                };
            }
            catch (Exception exception)
            {
                Console.Write(exception);
                return new UserRemoveResponse
                {
                    Success = false
                };
            }
        }
        
        public AssetRemoveResponse RemoveAsset(AssetRemoveRequest request)
        {
            if (request == null || request.AssetId == default(int))
            {
                return new AssetRemoveResponse()
                {
                    Success = false
                };
            }

            var asset = _applicationContext.Assets.Single(q => q.Id == request.AssetId);

            if (asset.isAssigned)
            {
                return new AssetRemoveResponse()
                {
                    Success = false
                };
            }

            try
            {
                _applicationContext.Remove(asset);
                _applicationContext.SaveChanges();
                return new AssetRemoveResponse
                {
                    Success = true
                };
            }
            catch (Exception exception)
            {
                Console.Write(exception);
                return new AssetRemoveResponse
                {
                    Success = false
                };
            }
        }

        public DebitRemoveResponse RemoveDebit(DebitRemoveRequest request)
        {
            if (request == null || request.DebitId == default(int))
            {
                return new DebitRemoveResponse()
                {
                    Success = false
                };
            }
            
            try
            {
                var debit = _applicationContext.Debits.Single(q => q.Id == request.DebitId);

                if (!debit.isDelivered)
                {
                    return new DebitRemoveResponse
                    {
                        Success = false
                    };
                }

                _applicationContext.Remove(debit);
                _applicationContext.SaveChanges();
                return new DebitRemoveResponse
                {
                    Success = true
                };
            }
            catch (Exception exception)
            {
                Console.Write(exception);
                return new DebitRemoveResponse
                {
                    Success = false
                };
            }
        }

        public UserUpdateResponse UpdateUser(UserUpdateRequest request)
        {
            if(request == null || request.UserId == default(int))
            {
                return new UserUpdateResponse
                {
                    Success = false
                };
            }

            var existingUser = _applicationContext.Users.FirstOrDefault(q => q.Id == request.UserId);

            if(existingUser == null)
            {
                return new UserUpdateResponse
                {
                    Success = false
                };
            }

            var isExistingAnotherUserWithSameEmail = _applicationContext.Users.FirstOrDefault(q => q.Id != request.UserId && q.Email == request.Email);

            if (isExistingAnotherUserWithSameEmail != null)
            {
                return new UserUpdateResponse
                {
                    Success = false
                };
            }

            var isExistingAnotherUserWithSameTelephoneNumber = _applicationContext.Users.FirstOrDefault(q => q.Id != request.UserId && q.TelephoneNumber == request.TelephoneNumber);


            if (isExistingAnotherUserWithSameTelephoneNumber != null)
            {
                return new UserUpdateResponse
                {
                    Success = false
                };
            }
            
            existingUser.Name = request.Name;
            existingUser.Surname = request.Surname;
            existingUser.Email = request.Email;
            existingUser.TelephoneNumber = request.TelephoneNumber;

            _applicationContext.SaveChanges();

            return new UserUpdateResponse
            {
                Success = true
            };
        }

        public AssetUpdateResponse UpdateAsset(AssetUpdateRequest request)
        {
            if (request == null || request.AssetId == default(int))
            {
                return new AssetUpdateResponse
                {
                    Success = false
                };
            }

            var existingAsset = _applicationContext.Assets.FirstOrDefault(q => q.Id == request.AssetId);

            if (existingAsset == null)
            {
                return new AssetUpdateResponse
                {
                    Success = false
                };
            }

            if (request.Type.ToLower().Equals("insan kaynağı"))
            {
                var isExistingAnotherHRAsset = _applicationContext.Assets.Any(q => q.Id != request.AssetId &&
                                                                                   q.PersonEmail == request.PersonEmail);

                if (isExistingAnotherHRAsset)
                {
                    return new AssetUpdateResponse
                    {
                        Success = false
                    };
                }
            }

            var isExistingAnotherAsset = _applicationContext.Assets.FirstOrDefault(q => q.Id != request.AssetId &&
                                                                                        q.Description == request.Description);

            if (isExistingAnotherAsset != null)
            {
                return new AssetUpdateResponse
                {
                    Success = false
                };
            }

            if(!existingAsset.isAssigned && request.isAssigned)
            {
                return new AssetUpdateResponse
                {
                    Success = false
                };
            }

            existingAsset.Type = request.Type;
            existingAsset.Name = request.Name;
            existingAsset.Description = request.Description;
            existingAsset.ExpiryDate = request.ExpiryDate == 0 ? existingAsset.ExpiryDate : request.ExpiryDate;
            existingAsset.PersonName = request.PersonName;
            existingAsset.PersonSurname = request.PersonSurname;
            existingAsset.PersonEmail = request.PersonEmail;

            bool isAssignedValueBeforeUpdate = existingAsset.isAssigned;
            
            existingAsset.isAssigned = request.isAssigned;

            if (isAssignedValueBeforeUpdate && !request.isAssigned)
            {
                try
                {
                    _applicationContext.Debits.FirstOrDefault(q => q.AssetId == request.AssetId).isDelivered = true;
                }
                catch (Exception exception)
                {
                    Console.Write(exception);
                    return new AssetUpdateResponse
                    {
                        Success = false
                    };
                }
            }

            _applicationContext.SaveChanges();

            return new AssetUpdateResponse
            {
                Success = true
            };
        }

        public DebitUpdateResponse UpdateDebit(DebitUpdateRequest request)
        {
            if (request == null || request.DebitId == default(int))
            {
                return new DebitUpdateResponse
                {
                    Success = false
                };
            }

            var existingDebit = _applicationContext.Debits.FirstOrDefault(q => q.Id == request.DebitId);

            if (existingDebit == null)
            {
                return new DebitUpdateResponse
                {
                    Success = false
                };
            }

            existingDebit.EndDate = request.EndDate == 0 ? existingDebit.EndDate : request.EndDate;
            existingDebit.EditedDate = DateTimeExtension.ToUnixTime(DateTime.Now);

            bool isDeliveredValueBeforeUpdate = existingDebit.isDelivered;

            existingDebit.isDelivered = request.isDelivered;

            if (isDeliveredValueBeforeUpdate && !request.isDelivered)
            {
                try
                {
                    _applicationContext.Assets.FirstOrDefault(q => q.Id == request.AssetId).isAssigned = true;
                }
                catch (Exception exception)
                {
                    Console.Write(exception);
                    return new DebitUpdateResponse
                    {
                        Success = false
                    };
                }
            }
            else if (!isDeliveredValueBeforeUpdate && request.isDelivered)
            {
                try
                {
                    _applicationContext.Assets.FirstOrDefault(q => q.Id == request.AssetId).isAssigned = false;
                }
                catch (Exception exception)
                {
                    Console.Write(exception);
                    return new DebitUpdateResponse
                    {
                        Success = false
                    };
                }
            }

            _applicationContext.SaveChanges();

            return new DebitUpdateResponse
            {
                Success = true
            };
        }

        public GetAllBaseResponse<UserGetAllResponse> GetAllUsers(UserGetAllRequest request)
        {
            if (request == null)
            {
                return new GetAllBaseResponse<UserGetAllResponse>()
                {
                    Success = false
                };
            }

            if (!string.IsNullOrEmpty(request.SearchQuery))
            {
                var userRecords = _applicationContext.Users.AsQueryable();
                userRecords = userRecords.Where(q => q.Name.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.Surname.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.Email.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.TelephoneNumber.ToLower().Contains(request.SearchQuery.ToLower()));

                if(request.PageNumber == default(int) && request.PageNumber == default(int))
                {
                    var searchedAllUsersList = userRecords
                    .Select(q => new UserGetAllResponse
                    {
                        Id = q.Id,
                        Name = q.Name,
                        Surname = q.Surname,
                        Email = q.Email,
                        TelephoneNumber = q.TelephoneNumber
                    })
                    .OrderBy(q => q.Name)
                    .ToList();

                    int allUsersDataCount = searchedAllUsersList.Count;

                    return new GetAllBaseResponse<UserGetAllResponse>
                    {
                        Success = true,
                        RecordList = searchedAllUsersList,
                        DataCount = allUsersDataCount,
                        PageCount = PagingHelper.PageCounter(allUsersDataCount)
                    };
                }
                var searchedUserList = userRecords
                    .Select(q => new UserGetAllResponse
                    {
                        Id = q.Id,
                        Name = q.Name,
                        Surname = q.Surname,
                        Email = q.Email,
                        TelephoneNumber = q.TelephoneNumber
                    })
                    .OrderBy(q => q.Name)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();
                
                int dataCount = searchedUserList.Count;
                
                return new GetAllBaseResponse<UserGetAllResponse>
                {
                    Success = true,
                    RecordList = searchedUserList,
                    DataCount = dataCount,
                    PageCount = PagingHelper.PageCounter(dataCount)
                };

            }

            if (request.PageNumber == default(int) && request.PageNumber == default(int) && string.IsNullOrEmpty(request.SearchQuery))
            {
                var allUsersList = _applicationContext.Users
                    .Select(q => new UserGetAllResponse { 
                        Id = q.Id, 
                        Name = q.Name,
                        Surname = q.Surname,
                        Email = q.Email,
                        TelephoneNumber = q.TelephoneNumber
                    })
                    .OrderBy(q => q.Name)
                    .ToList();
                
                int dataCount = allUsersList.Count;

                return new GetAllBaseResponse<UserGetAllResponse>
                {
                    Success = true,
                    RecordList = allUsersList,
                    DataCount = dataCount,
                    PageCount = PagingHelper.PageCounter(dataCount)
                };
            }

            var usersList = _applicationContext.Users
                    .Select(q => new UserGetAllResponse
                    {
                        Id = q.Id,
                        Name = q.Name,
                        Surname = q.Surname,
                        Email = q.Email,
                        TelephoneNumber = q.TelephoneNumber
                    })
                    .OrderBy(q => q.Name)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

            return new GetAllBaseResponse<UserGetAllResponse>
            { 
                Success = true,
                RecordList = usersList 
            };
        }

        public GetAllBaseResponse<AssetGetAllResponse> GetAllAssets(AssetGetAllRequest request)
        {
            if (request == null)
            {
                return new GetAllBaseResponse<AssetGetAllResponse>()
                {
                    Success = false
                };
            }

            if (!string.IsNullOrEmpty(request.SearchQuery))
            {
                var assetRecords = _applicationContext.Assets.AsQueryable();
                assetRecords = assetRecords.Where(q => q.Type.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.Name.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.Description.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.PersonName.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.PersonSurname.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.PersonEmail.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.isAssigned.ToString().ToLower().Contains(request.SearchQuery.ToLower()));

                if (request.PageNumber == default(int) && request.PageNumber == default(int))
                {
                    var searchedAssetsQueryable = assetRecords
                    .Select(q => new AssetGetAllResponse
                    {
                        Id = q.Id,
                        Type = q.Type,
                        Name = q.Name,
                        Description = q.Description,
                        AddedDate = DateTimeExtension.FromUnixTime(q.AddedDate).ToString(),
                        ExpiryDate = q.ExpiryDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.ExpiryDate).ToString(),
                        PersonName = q.PersonName,
                        PersonSurname = q.PersonSurname,
                        PersonEmail = q.PersonEmail,
                        isAssigned = q.isAssigned
                    })
                    .OrderBy(q => q.Type)
                    .AsQueryable();

                    var searchedAssetsList = FilterAssets(searchedAssetsQueryable, request.FilterByType, request.FilterByIsAssigned, request.SortByName);

                    int dataCount = searchedAssetsList.Count;

                    return new GetAllBaseResponse<AssetGetAllResponse>
                    {
                        Success = true,
                        RecordList = searchedAssetsList,
                        DataCount = dataCount,
                        PageCount = PagingHelper.PageCounter(dataCount)
                    };
                }
                else
                {
                    var searchedAssetsQueryable = assetRecords
                    .Select(q => new AssetGetAllResponse
                    {
                        Id = q.Id,
                        Type = q.Type,
                        Name = q.Name,
                        Description = q.Description,
                        AddedDate = DateTimeExtension.FromUnixTime(q.AddedDate).ToString(),
                        ExpiryDate = q.ExpiryDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.ExpiryDate).ToString(),
                        PersonName = q.PersonName,
                        PersonSurname = q.PersonSurname,
                        PersonEmail = q.PersonEmail,
                        isAssigned = q.isAssigned
                    })
                    .OrderBy(q => q.Name);

                    var searchedAssetsList = FilterAssetsWithoutPaging(searchedAssetsQueryable, request.FilterByType, request.FilterByIsAssigned, request.SortByName)
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToList();
                    
                    int dataCount = searchedAssetsList.Count;

                    return new GetAllBaseResponse<AssetGetAllResponse>
                    {
                        Success = true,
                        RecordList = searchedAssetsList,
                        DataCount = dataCount,
                        PageCount = PagingHelper.PageCounter(dataCount)
                    };
                }
            }

            if (request.PageNumber == default(int) && request.PageNumber == default(int) && string.IsNullOrEmpty(request.SearchQuery))
            {
                var allAssetsQueryable = _applicationContext.Assets
                    .Select(q => new AssetGetAllResponse
                    {
                        Id = q.Id,
                        Type = q.Type,
                        Name = q.Name,
                        Description = q.Description,
                        AddedDate = DateTimeExtension.FromUnixTime(q.AddedDate).ToString(),
                        ExpiryDate = q.ExpiryDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.ExpiryDate).ToString(),
                        PersonName = q.PersonName,
                        PersonSurname = q.PersonSurname,
                        PersonEmail = q.PersonEmail,
                        isAssigned = q.isAssigned
                    })
                    .OrderBy(q => q.Type)
                    .AsQueryable();

                var allAssetsList = FilterAssets(allAssetsQueryable, request.FilterByType, request.FilterByIsAssigned, request.SortByName);

                int dataCount = allAssetsList.Count;

                return new GetAllBaseResponse<AssetGetAllResponse>
                {
                    Success = true,
                    RecordList = allAssetsList,
                    DataCount = dataCount,
                    PageCount = PagingHelper.PageCounter(dataCount)
                };
            }
            else
            {
                var assetsQueryable = _applicationContext.Assets
                    .Select(q => new AssetGetAllResponse
                    {
                        Id = q.Id,
                        Type = q.Type,
                        Name = q.Name,
                        Description = q.Description,
                        AddedDate = DateTimeExtension.FromUnixTime(q.AddedDate).ToString(),
                        ExpiryDate = q.ExpiryDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.ExpiryDate).ToString(),
                        PersonName = q.PersonName,
                        PersonSurname = q.PersonSurname,
                        PersonEmail = q.PersonEmail,
                        isAssigned = q.isAssigned
                    })
                    .OrderBy(q => q.Type);
                    
                    
                var assetsList = FilterAssets(assetsQueryable, request.FilterByType, request.FilterByIsAssigned, request.SortByName)
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToList();

                int dataCount = assetsList.Count;

                return new GetAllBaseResponse<AssetGetAllResponse>
                {
                    Success = true,
                    RecordList = assetsList,
                    DataCount = dataCount,
                    PageCount = PagingHelper.PageCounter(dataCount)
                };
            }
        }

        public GetAllBaseResponse<DebitGetAllResponse> GetAllDebits(DebitGetAllRequest request)
        {
            if (request == null)
            {
                return new GetAllBaseResponse<DebitGetAllResponse>()
                {
                    Success = false
                };
            }

            if (!string.IsNullOrEmpty(request.SearchQuery))
            {
                var debitRecords = _applicationContext.Debits.Include(a => a.Admin).Include(u => u.User).AsQueryable();
                debitRecords = debitRecords.Where(q => q.Admin.Name.ToLower().Contains(request.SearchQuery.ToLower()) || 
                                                q.Admin.Surname.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.User.Name.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.User.Surname.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.Asset.Type.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.Asset.Name.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.Asset.Description.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.Cause.ToLower().Contains(request.SearchQuery.ToLower()) ||
                                                q.isDelivered.ToString().ToLower().Contains(request.SearchQuery.ToLower()));

                if (request.PageNumber == default(int) && request.PageNumber == default(int))
                {
                    var searchedAllDebitsQueryable = debitRecords
                    .Select(q => new DebitGetAllResponse
                    {
                        Id = q.Id,
                        AssetId = q.AssetId,
                        Assigner = string.Concat(q.Admin.Name, " ", q.Admin.Surname),
                        User = string.Concat(q.User.Name, " ", q.User.Surname),
                        AssetType = q.Asset.Type,
                        AssetName = q.Asset.Name,
                        AssetDescription = q.Asset.Description,
                        Cause = q.Cause,
                        StartDate = DateTimeExtension.FromUnixTime(q.StartDate).ToString(),
                        EndDate = q.EndDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.EndDate).ToString(),
                        CreatedDate = DateTimeExtension.FromUnixTime(q.CreatedDate).ToString(),
                        EditedDate = q.EditedDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.EditedDate).ToString(),
                        isDelivered = q.isDelivered
                    })
                    .OrderBy(q => q.isDelivered)
                    .AsQueryable();

                    var searchedAllDebitsList = FilterDebits(searchedAllDebitsQueryable, request.FilterByType, request.FilterByIsDelivered, request.SortByAssetName);

                    int allDebitsDataCount = searchedAllDebitsList.Count;

                    return new GetAllBaseResponse<DebitGetAllResponse>
                    {
                        Success = true,
                        RecordList = searchedAllDebitsList,
                        DataCount = allDebitsDataCount,
                        PageCount = PagingHelper.PageCounter(allDebitsDataCount)
                    };
                }
                else
                {
                    var searchedDebitQueryable = debitRecords
                    .Select(q => new DebitGetAllResponse
                    {
                        Id = q.Id,
                        AssetId = q.AssetId,
                        Assigner = string.Concat(q.Admin.Name, " ", q.Admin.Surname),
                        User = string.Concat(q.User.Name, " ", q.User.Surname),
                        AssetType = q.Asset.Type,
                        AssetName = q.Asset.Name,
                        AssetDescription = q.Asset.Description,
                        Cause = q.Cause,
                        StartDate = DateTimeExtension.FromUnixTime(q.StartDate).ToString(),
                        EndDate = q.EndDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.EndDate).ToString(),
                        CreatedDate = DateTimeExtension.FromUnixTime(q.CreatedDate).ToString(),
                        EditedDate = q.EditedDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.EditedDate).ToString(),
                        isDelivered = q.isDelivered
                    })
                    .OrderBy(q => q.isDelivered);
                    
                    var searchedDebitList = FilterDebitsWithoutPaging(searchedDebitQueryable, request.FilterByType, request.FilterByIsDelivered, request.SortByAssetName)
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToList();

                    int dataCount = searchedDebitList.Count;

                    return new GetAllBaseResponse<DebitGetAllResponse>
                    {
                        Success = true,
                        RecordList = searchedDebitList,
                        DataCount = dataCount,
                        PageCount = PagingHelper.PageCounter(dataCount)
                    };
                }
            }

            if (request.PageNumber == default(int) && request.PageNumber == default(int) && string.IsNullOrEmpty(request.SearchQuery))
            {
                var allDebitsQueryable = _applicationContext.Debits
                    .Include(a => a.Admin)
                    .Include(u => u.User)
                    .Select(q => new DebitGetAllResponse
                    {
                        Id = q.Id,
                        AssetId = q.AssetId,
                        Assigner = string.Concat(q.Admin.Name, " ", q.Admin.Surname),
                        User = string.Concat(q.User.Name, " ", q.User.Surname),
                        AssetType = q.Asset.Type,
                        AssetName = q.Asset.Name,
                        AssetDescription = q.Asset.Description,
                        Cause = q.Cause,
                        StartDate = DateTimeExtension.FromUnixTime(q.StartDate).ToString(),
                        EndDate = q.EndDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.EndDate).ToString(),
                        CreatedDate = DateTimeExtension.FromUnixTime(q.CreatedDate).ToString(),
                        EditedDate = q.EditedDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.EditedDate).ToString(),
                        isDelivered = q.isDelivered
                    })
                    .OrderBy(q => q.isDelivered);
                
                var allDebitsList = FilterDebits(allDebitsQueryable, request.FilterByType, request.FilterByIsDelivered, request.SortByAssetName);

                int dataCount = allDebitsList.Count;

                return new GetAllBaseResponse<DebitGetAllResponse>
                {
                    Success = true,
                    RecordList = allDebitsList,
                    DataCount = dataCount,
                    PageCount = PagingHelper.PageCounter(dataCount)
                };
            }
            else
            {
                var debitsQueryable = _applicationContext.Debits
                    .Include(a => a.Admin)
                    .Include(u => u.User)
                    .Select(q => new DebitGetAllResponse
                    {
                        Id = q.Id,
                        AssetId = q.AssetId,
                        Assigner = string.Concat(q.Admin.Name, " ", q.Admin.Surname),
                        User = string.Concat(q.User.Name, " ", q.User.Surname),
                        AssetType = q.Asset.Type,
                        AssetName = q.Asset.Name,
                        AssetDescription = q.Asset.Description,
                        Cause = q.Cause,
                        StartDate = DateTimeExtension.FromUnixTime(q.StartDate).ToString(),
                        EndDate = q.EndDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.EndDate).ToString(),
                        CreatedDate = DateTimeExtension.FromUnixTime(q.CreatedDate).ToString(),
                        EditedDate = q.EditedDate == 0 ? "0" : DateTimeExtension.FromUnixTime(q.EditedDate).ToString(),
                        isDelivered = q.isDelivered
                    })
                    .OrderBy(q => q.isDelivered);

                var debitsList = FilterDebitsWithoutPaging(debitsQueryable, request.FilterByType, request.FilterByIsDelivered, request.SortByAssetName)
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToList();

                int dataCount = debitsList.Count;

                return new GetAllBaseResponse<DebitGetAllResponse>
                {
                    Success = true,
                    RecordList = debitsList,
                    DataCount = dataCount,
                    PageCount = PagingHelper.PageCounter(dataCount)
                };
            }
        }

        public List<AssetGetAllResponse> FilterAssets(IQueryable<AssetGetAllResponse> queryable, string filterByType, string filterByIsAssigned, string sortByName)
        {
            var _queryable = queryable;

            if (!string.IsNullOrEmpty(filterByType))
            {
                _queryable = _queryable.Where(q => q.Type.Contains(filterByType));
            }

            if (!string.IsNullOrEmpty(filterByIsAssigned))
            {
                _queryable = _queryable.Where(q => q.isAssigned.ToString().ToLower().Contains(filterByIsAssigned.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortByName))
            {
                if (sortByName.Equals("Artan"))
                {
                    _queryable = _queryable.OrderBy(q => q.Name);
                }
                else if (sortByName.Equals("Azalan"))
                {
                    _queryable = _queryable.OrderByDescending(q => q.Name);
                }
            }

            return _queryable.ToList();
        }

        public IQueryable<AssetGetAllResponse> FilterAssetsWithoutPaging(IQueryable<AssetGetAllResponse> queryable, string filterByType, string filterByIsAssigned, string sortByName)
        {
            var _queryable = queryable;

            if (!string.IsNullOrEmpty(filterByType))
            {
                _queryable = _queryable.Where(q => q.Type.Contains(filterByType));
            }

            if (!string.IsNullOrEmpty(filterByIsAssigned))
            {
                _queryable = _queryable.Where(q => q.isAssigned.ToString().ToLower().Contains(filterByIsAssigned.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortByName))
            {
                if (sortByName.Equals("Artan"))
                {
                    _queryable = _queryable.OrderBy(q => q.Name);
                }
                else if (sortByName.Equals("Azalan"))
                {
                    _queryable = _queryable.OrderByDescending(q => q.Name);
                }
            }

            return _queryable;
        }

        public List<DebitGetAllResponse> FilterDebits(IQueryable<DebitGetAllResponse> queryable, string filterByType, string filterByIsDelivered, string sortByAssetName)
        {
            var _queryable = queryable;

            if (!string.IsNullOrEmpty(filterByType))
            {
                _queryable = _queryable.Where(q => q.AssetType.Contains(filterByType));
            }

            if (!string.IsNullOrEmpty(filterByIsDelivered))
            {
                _queryable = _queryable.Where(q => q.isDelivered.ToString().ToLower().Contains(filterByIsDelivered.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortByAssetName))
            {
                if (sortByAssetName.Equals("Artan"))
                {
                    _queryable = _queryable.OrderBy(q => q.AssetName);
                }
                else if (sortByAssetName.Equals("Azalan"))
                {
                    _queryable = _queryable.OrderByDescending(q => q.AssetName);
                }
            }

            return _queryable.ToList();
        }

        public IQueryable<DebitGetAllResponse> FilterDebitsWithoutPaging(IQueryable<DebitGetAllResponse> queryable, string filterByType, string filterByIsDelivered, string sortByAssetName)
        {
            var _queryable = queryable;

            if (!string.IsNullOrEmpty(filterByType))
            {
                _queryable = _queryable.Where(q => q.AssetType.Contains(filterByType));
            }

            if (!string.IsNullOrEmpty(filterByIsDelivered))
            {
                _queryable = _queryable.Where(q => q.isDelivered.ToString().ToLower().Contains(filterByIsDelivered.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortByAssetName))
            {
                if (sortByAssetName.Equals("Artan"))
                {
                    _queryable = _queryable.OrderBy(q => q.AssetName);
                }
                else if (sortByAssetName.Equals("Azalan"))
                {
                    _queryable = _queryable.OrderByDescending(q => q.AssetName);
                }
            }

            return _queryable;
        }
    }
}