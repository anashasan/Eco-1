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
                    CreatedOn = DateTime.Now,
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
                         CreatedOn = i.CreatedOn,
                         Name = i.Name,
                         Phone = i.Phone,
                         
                       }).ToList();
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
        public BranchDto GetBranchById(int id)
        {
            try
            {
                return _context.Branch
                       .AsNoTracking()
                       .Where(p => p.PkBranchId == id)
                       .Select(i => new BranchDto
                       {
                           Address = i.Address,
                           BranchId = i.PkBranchId,
                           Email = i.Email,
                           CreatedOn = i.CreatedOn,
                           Name = i.Name,
                           Phone = i.Phone,

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
        public async Task<int> UpdateBranch(BranchDto requestDto)
        {
            try
            {
                var branch = new Branch
                {
                    PkBranchId = requestDto.BranchId,
                    Name = requestDto.Name,
                    Address = requestDto.Address,
                    Email = requestDto.Email,
                    Phone = requestDto.Phone,
                    CreatedOn = DateTime.Now,
                };
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
