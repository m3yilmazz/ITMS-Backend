using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITMSWebAPI.Services.Interfaces
{
    public interface ISessionInfoService
    {
        string AssignSessionInfo(int AdminId);
        bool RemoveSessionInfo(string guid);
        bool SessionExistanceChecker(string guid);
    }
}
