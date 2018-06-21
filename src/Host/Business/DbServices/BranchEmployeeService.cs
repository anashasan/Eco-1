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
    public class BranchEmployeeService : IBranchEmployeeService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly EcoDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public BranchEmployeeService(EcoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> AddBranchEmployee(BranchEmployeeDto requestDto)
        {
            try
            {
                var model = new BranchEmployee
                {
                    EmployeeName = requestDto.EmployeeName,
                    Designation = requestDto.Designation,
                    Email = requestDto.Email,
                    Phone = requestDto.Phone,
                    FkBranchId = requestDto.BranchId
                };

                _context.BranchEmployee.Add(model);
                _context.SaveChanges();
                return await Task.FromResult(model.PkBranchEmployeeId);

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
        public List<BranchEmployeeDto> GetAllBranchEmployee()
        {
            try
            {
                return _context.BranchEmployee
                       .AsNoTracking()
                       .Select(i => new BranchEmployeeDto
                       {
                           BranchEmployeeId = i.PkBranchEmployeeId,
                           Designation = i.Designation,
                           Email = i.Email,
                           EmployeeName = i.EmployeeName,
                           Phone = i.Phone,
                           BranchName = i.FkBranch.Name,
                           CompanyName = i.FkBranch.CompanyBranch.Select(o => o.FkCompany.Name).Single()
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
        public BranchEmployeeDto GetBranchEmployeeById(int id)
        {
            try
            {
                var model = _context.BranchEmployee
                            .AsNoTracking()
                            .Where(o => o.PkBranchEmployeeId == id)
                            .Select(i => new BranchEmployeeDto
                            {
                                Email = i.Email,
                                EmployeeName = i.EmployeeName,
                                Designation = i.Designation,
                                BranchId = i.FkBranchId,
                                CompanyId = i.FkBranch.CompanyBranch.Select(p => p.FkCompanyId).Single(),
                                BranchEmployeeId = i.PkBranchEmployeeId,
                                Phone = i.Phone,
                            }).SingleOrDefault();

                return model;
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
        public async Task<int> UpdateBranchEmployee(BranchEmployeeDto requestDto)
        {
            try
            {
                var model = new BranchEmployee
                {
                    PkBranchEmployeeId = requestDto.BranchEmployeeId.Value,
                    EmployeeName = requestDto.EmployeeName,
                    Designation = requestDto.Designation,
                    Email = requestDto.Email,
                    Phone = requestDto.Phone,
                    FkBranchId = requestDto.BranchId
                };

                _context.BranchEmployee.Update(model);
                _context.SaveChanges();
                return await Task.FromResult(model.PkBranchEmployeeId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<BranchEmployeeDto> GetBranchEmployeeByBranchId(int id)
        {
            try
            {
                var branchemployee = _context.BranchEmployee
                    .AsNoTracking()
                    .Where(i => i.FkBranchId == id)
                    .Select(a => new BranchEmployeeDto
                    {
                        BranchEmployeeId = a.PkBranchEmployeeId,
                        //CompanyName = a.FkBranch.CompanyBranch.Select(p => p.FkCompany.Name).SingleOrDefault(),
                        BranchName = a.FkBranch.Name,
                        EmployeeName = a.EmployeeName,
                        Designation = a.Designation,
                        Email = a.Email,
                        Phone = a.Phone


                    }).ToList();
                return branchemployee;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public List<BranchEmployeeDto> GetBranchEmployeeByCompanyId(int id)
        {
            try
            {
                var branchemployee = _context.BranchEmployee
                   .AsNoTracking()
                   .Where(i => i.FkBranch.CompanyBranch.Select(p => p.FkCompanyId).Single() == id)
                   .Select(a => new BranchEmployeeDto
                   {
                       BranchEmployeeId = a.PkBranchEmployeeId,
                       CompanyName = a.FkBranch.CompanyBranch.Select(p => p.FkCompany.Name).SingleOrDefault(),
                       BranchName = a.FkBranch.Name,
                       EmployeeName = a.EmployeeName,
                       Designation = a.Designation,
                       Email = a.Email,
                       Phone = a.Phone


                   }).ToList();
                return branchemployee;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task<int> DeleteBranchEmployeeById(int id)

        {
            try
            {
                var branchEmployee = _context.BranchEmployee.Find(id);
                _context.BranchEmployee.Remove(branchEmployee);
                return Task.FromResult(_context.SaveChanges());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
