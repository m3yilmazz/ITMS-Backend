using ITMSWebAPI.Models;
using ITMSWebAPI.Models.Context;
using ITMSWebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITMSWebAPI.Services.Implementations
{
    public class SessionInfoService : ISessionInfoService
    {
        private readonly ApplicationContext _applicationContext;
        public SessionInfoService(ApplicationContext applicationContext) : base()
        {
            _applicationContext = applicationContext;
        }
        public string AssignSessionInfo(int AdminId)
        {
            using (var transaction = _applicationContext.Database.BeginTransaction())
            {
                try
                {
                    string guid = Guid.NewGuid().ToString();

                    _applicationContext.SessionInfos.Add(new SessionInfo()
                    {
                        AdminId = AdminId,
                        SessionGuid = guid
                    });
                    _applicationContext.SaveChanges();

                    transaction.Commit();

                    return guid;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    System.Console.Write(exception);
                    return null;
                }
            }
        }

        public bool RemoveSessionInfo(string guid)
        {
            try
            {
                _applicationContext.Remove(_applicationContext.SessionInfos.Single(q => q.SessionGuid == guid));
                _applicationContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                System.Console.Write(exception);
                return false;
            }
            
        }

        public bool SessionExistanceChecker(string guid)
        {
            bool isSessionExist = _applicationContext.SessionInfos.Any(q => q.SessionGuid == guid);
            if (isSessionExist)
            {
                return true;
            } else
            {
                return false;
            }
            
        }
    }
}
