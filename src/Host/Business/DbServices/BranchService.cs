using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Host.Business.DbServices
{
    public class BranchService : IBranchService
    {
        private readonly EcoDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public BranchService(EcoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
    /*    public async Task<int> AddBranch(BranchDto requestDto)
        {
            try
            {
                var branch = new Branch
                {
                    Name = requestDto.Name,
                    Address = requestDto.Address,
                    Email = requestDto.Email,
                    Phone = requestDto.Phone,
                    Location=requestDto.Location,
                    CreatedOn = DateTime.Now,
                    CompanyBranch = new List<CompanyBranch>
                    
                    {
                        new CompanyBranch
                        {
                             FkCompanyId = requestDto.CompanyId,

                             
                        }

                    }

                };
                _context.Branch.Add(branch);
                _context.SaveChanges();
                return await Task.FromResult(branch.PkBranchId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        */

        public async Task<int> AddBranch(BranchDto requestDto)
        {

            try
            {
                var branch = new Branch
                {
                    Name = requestDto.Name,
                    Address = requestDto.Address,
                    Email = requestDto.Email,
                    Phone = requestDto.Phone,
                    Location = requestDto.Location,
                    CreatedOn = DateTime.Now,

                };

                _context.Branch.Add(branch);
                _context.SaveChanges();

                var CompanyBranch = new CompanyBranch
                {
                    FkCompanyId = requestDto.CompanyId,
                    FkBranchId = branch.PkBranchId,
                };

                _context.CompanyBranch.Add(CompanyBranch);

                return await Task.FromResult(_context.SaveChanges());

}

            catch (Exception e)

            {
                Console.WriteLine(e);

                throw;
            }
        }

        public void DeleteBranch(int branchId)
        {
            var connection = _context.Database.GetDbConnection();
            connection.Execute(
                   "[dbo].[usp_DeleteBranch]"
                   , new { @paramBranchId = branchId }
                   ,commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BranchDto> GetAllBranch()
        {

            try
            {
                return _context.Branch
                       .AsNoTracking()
                       .Select(i => new BranchDto
                       {
                           Address = i.Address,
                           BranchId = i.PkBranchId,
                           Email = i.Email,
                           Location = i.Location,
                           CreatedOn = i.CreatedOn,
                           Name = i.Name,
                           Phone = i.Phone,
                           CompanyName = i.CompanyBranch.Select(p => p.FkCompany.Name).SingleOrDefault()

                       })
                       .OrderBy(i => i.CompanyName)
                       .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

      /*  public SelectList GetBranchByCompanyId(int id)
        {
            try
            {
                var model = _context.CompanyBranch
                            .AsNoTracking()
                            .Where(i => i.FkCompanyId == id)
                            .Select(p => new BranchDto
                            {
                                BranchId = p.FkBranch.PkBranchId,
                                Name=p.FkBranch.Name,
                            }).ToList();

                return new SelectList(model, "BranchId", "BranchName");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }*/

        public List<BranchDto> GetBranchByCompanyId(int id)
        {
            try
            {
                var companybranch = _context.CompanyBranch
                    .AsNoTracking()
                    .Where(i => i.FkCompanyId == id)
                    .Select(i => new BranchDto
                    {
                        BranchId = i.FkBranch.PkBranchId,
                        CompanyName=i.FkCompany.Name,
                        Name = i.FkBranch.Name,
                        Phone = i.FkBranch.Phone,
                        Email = i.FkBranch.Email,
                        Location = i.FkBranch.Location,
                        Address = i.FkBranch.Address
                    }).ToList();
                return companybranch;
            }
            catch (Exception e )
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
       //public List<BranchDto> GetBranchByBranchEmployeeId(int id)
       // {
       //     try
       //     {
       //         var branchemployee=_context.BranchEmployee
       //             .AsNoTracking()
       //             .Where(i =>i.PkBranchEmployeeId ==id)
       //             .Select 
       //     }
       //     catch (Exception e)
       //     {
       //         Console.WriteLine(e);
       //         throw;
       //     }
       // }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BranchDto GetBranchById(int id)
        {
            try
            {
                return _context.Branch
                       .AsNoTracking()
                       .Where(p => p.PkBranchId == id)
                       .Select(i => new BranchDto
                       {
                          // CompanyId = i.CompanyBranch.Select(p => p.FkCompanyId).Single(),
                           Address = i.Address,
                           BranchId = i.PkBranchId,
                           Email = i.Email,
                           Location = i.Location,
                           CreatedOn = i.CreatedOn,
                           Name = i.Name,
                           Phone = i.Phone,

                       }).SingleOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<BranchDto> GetBranchByLocationId(int id)
        {
            try
            {

                var Locationbranch = _context.BranchLocation
                    .AsNoTracking()
                    .Where(i => i.FkLocationId == id)
                    .Select(i => new BranchDto
                    {
                        BranchId = i.FkBranch.PkBranchId,
                        Name = i.FkBranch.Name,
                        Location =i.FkBranch.Location,
                        Address =i.FkBranch.Address,
                        Phone =i.FkBranch.Phone,
                        Email=i.FkBranch.Email,
                        
                    }).ToList();
                return Locationbranch;
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
        public async Task<int> UpdateBranch(BranchDto requestDto)
        {
            try
            {
                var branch = _context.Branch.Find(requestDto.BranchId);
                branch.PkBranchId = requestDto.BranchId;

                branch.Name = requestDto.Name;
                branch.Phone = requestDto.Phone;
                branch.Address = requestDto.Address;
                branch.Email = requestDto.Email;
                branch.Location = requestDto.Location;
                branch.CreatedOn = DateTime.Now;
                branch.UpdatedOn = DateTime.Now;
                
                //var branch = new Branch
                //{
                //    PkBranchId = requestDto.BranchId,
                //    Name = requestDto.Name,
                //    Address = requestDto.Address,
                //    Email = requestDto.Email,
                //    Location = requestDto.Location,
                //    Phone = requestDto.Phone,
                //    CreatedOn = DateTime.Now,
                    
                //};
                _context.Branch.Update(branch);
                _context.SaveChanges();
                return await Task.FromResult(branch.PkBranchId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
