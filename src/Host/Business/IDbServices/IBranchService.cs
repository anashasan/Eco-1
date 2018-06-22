using Host.DataModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface IBranchService
    {
        Task<int> AddBranch(BranchDto requestDto);
        Task<int> UpdateBranch(BranchDto requestDto);
        BranchDto GetBranchById(int id);
        List<BranchDto> GetAllBranch();
        List<BranchDto> GetBranchByCompanyId(int id);
        //List<BranchDto> GetBranchByBranchEmployeeId(int id);
       // SelectList GetBranchByCompanyId(int id);
    }
}
