using Host.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface IBranchEmployeeService
    {
        Task<int> AddBranchEmployee(BranchEmployeeDto requestDto);
        Task<int> UpdateBranchEmployee(BranchEmployeeDto requestDto);
        BranchEmployeeDto GetBranchEmployeeById(int id);
        List<BranchEmployeeDto> GetAllBranchEmployee();
        List<BranchEmployeeDto> GetBranchEmployeeByBranchId(int id);
        List<BranchEmployeeDto> GetBranchEmployeeByCompanyId(int id);
        Task<int> DeleteBranchEmployeeById(int id);
    }
}
