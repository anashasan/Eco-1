using Host.DataModel;
using Host.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface IEmployeeProfileService
    {
        Task<int> AddEmployeeProfile(EmployeeProfileDto requestDto);
        Task<int> UpdateEmployeeProfile(EmployeeProfileDto requestDto);
        EmployeeProfileDto GetEmployeeProfileById(int id);
        List<EmployeeProfileDto> GetAllEmployeeProfile();
        EmployeeProfileDto GetEmployeeProfileByUserId(string UserId);
        bool IsEmailExist(string email);
        bool IsUserNameExist(string email);
        UserInfoModel GetUserInfoByUserId(string userId);
    }
}
