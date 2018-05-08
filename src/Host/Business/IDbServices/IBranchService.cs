using Host.DataModel;
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
        List<CompanyBranchDto> GetBranchByCompanyId(int id);
    }
}
