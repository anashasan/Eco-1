using Host.DataModel;
using Host.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface IEmployeesService
    {
        List<UserInfoModel> GetAllEmployee();
        List<UserInfoModel> GetAllInActiveUser();
        string GetUserName(string userId);
        bool CheckEmailIsExist(string email);
        //Task<int> DeleteUser(string userId);
        void UserInActive(string userId);
        void UserActive(string userId);
    }
}
