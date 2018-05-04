using Host.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface ICompanyService
    {
        Task<int> AddCompany(CompanyDto requestDto);
        Task<int> UpdateCompany(CompanyDto requestDto);
        CompanyDto GetCompanyById(int id);
        Task<List<CompanyDto>> GetAllCompany();
        List<GetCompanyViewDto> GetCompanyList();

    }
}
