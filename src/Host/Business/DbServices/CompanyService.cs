using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.DbServices
{
    public class CompanyService : ICompanyService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly EcoDbContext _context;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public CompanyService(EcoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> AddCompany(CompanyDto requestDto)
        {
            try
            {
                var company = new Company
                {
                    Name= requestDto.Name,
                    Type = requestDto.Type,
                    Url = requestDto.Url,
                    CreatedOn = DateTime.Now
                };

                _context.Company.Add(company);
                _context.SaveChanges();
                return await Task.FromResult(company.PkCompanyId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyDto>> GetAllCompany()
        {
            try
            {
                return await Task.FromResult(_context.Company
                       .AsNoTracking()
                       .Select(p => new CompanyDto
                       {
                           CompanyId = p.PkCompanyId,
                           CreatedOn = p.CreatedOn,
                           Name = p.Name,
                           Type = p.Type,
                           Url = p.Url
                       }).ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyDto GetCompanyById(int id)
        {
            try
            {
                return _context.Company
                       .AsNoTracking()
                       .Where(i => i.PkCompanyId == id)
                       .Select(p => new CompanyDto
                       {
                           CompanyId = p.PkCompanyId,
                           CreatedOn = p.CreatedOn,
                           Name = p.Name,
                           Type = p.Type,
                           Url = p.Url
                       }).Single();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> UpdateCompany(CompanyDto requestDto)
        {
            try
            {
                var company = new Company
                {
                    PkCompanyId = requestDto.CompanyId,
                    Name = requestDto.Name,
                    Type = requestDto.Type,
                    Url = requestDto.Url,
                    CreatedOn = DateTime.Now
                };

                _context.Company.Update(company);
                _context.SaveChanges();
                return await Task.FromResult(company.PkCompanyId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
