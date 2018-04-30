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
    public class EmployeeProfileService : IEmployeeProfileService
    {
        private readonly EcoDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public EmployeeProfileService(EcoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> AddEmployeeProfile(EmployeeProfileDto requestDto)
        {
            try
            {
                var employeeProfile = new IEmployeeProfile
                {
                    FirstName = requestDto.FirstName,
                    LastName = requestDto.LastName,
                    CellPhone = requestDto.CellPhone,
                    City = requestDto.City,
                    DateOfBirth = requestDto.DateOfBirth,
                    HomePhone = requestDto.HomePhone,
                    MiddleInitial = requestDto.MiddleInitial,
                    JobTitle = requestDto.JobTitle,
                    State = requestDto.State,
                    StreetAddress = requestDto.StreetAddress,
                    FkInitiatedById = requestDto.FkInitiatedById,
                    WorkEmail = requestDto.WorkEmail,
                    ZipCode = requestDto.ZipCode,
                    FkGenderId = requestDto.GenderId,
                    FkUserId = requestDto.FkUserId,
                    CreatedOn = DateTime.Now,
                };
                _context.EmployeeProfile.Add(employeeProfile);
                _context.SaveChanges();
                return await Task.FromResult(employeeProfile.PkEmployeeProfileId);

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
        public List<EmployeeProfileDto> GetAllEmployeeProfile() =>
            _context.EmployeeProfile
                   .AsNoTracking()
                   .Select(p => new EmployeeProfileDto
                   {
                       EmployeeProfileId = p.PkEmployeeProfileId,
                       CellPhone = p.CellPhone,
                       City = p.City,
                       ZipCode = p.ZipCode,
                       DateOfBirth = p.DateOfBirth,
                       CreatedOn = p.CreatedOn,
                       HomePhone = p.HomePhone,
                       FirstName = p.FirstName,
                       GenderId = p.FkGenderId,
                       JobTitle = p.JobTitle,
                       MiddleInitial = p.MiddleInitial,
                       State = p.State,
                       LastName = p.LastName,
                       StreetAddress = p.StreetAddress,
                       WorkEmail = p.WorkEmail,
                       FkInitiatedById = p.FkInitiatedById,
                       FkUserId = p.FkUserId
                   }).ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmployeeProfileDto GetEmployeeProfileById(int id) => _context.EmployeeProfile
                   .AsNoTracking()
                   .Where(i => i.PkEmployeeProfileId == id)
                   .Select(p => new EmployeeProfileDto
                   {
                       EmployeeProfileId = p.PkEmployeeProfileId,
                       CellPhone = p.CellPhone,
                       City = p.City,
                       ZipCode = p.ZipCode,
                       DateOfBirth = p.DateOfBirth,
                       CreatedOn = p.CreatedOn,
                       HomePhone = p.HomePhone,
                       FirstName = p.FirstName,
                       GenderId = p.FkGenderId,
                       JobTitle = p.JobTitle,
                       MiddleInitial = p.MiddleInitial,
                       State = p.State,
                       LastName = p.LastName,
                       StreetAddress = p.StreetAddress,
                       WorkEmail = p.WorkEmail,
                       FkInitiatedById = p.FkInitiatedById,
                       FkUserId = p.FkUserId
                   }).Single();

        public EmployeeProfileDto GetEmployeeProfileByUserId(string UserId)
        {
            try
            {
                var employeeProfileModel = _context.EmployeeProfile
                                           .AsNoTracking()
                                           .Where(i => i.FkUserId == UserId)
                                           .Select(p => new EmployeeProfileDto
                                           {
                                               EmployeeProfileId = p.PkEmployeeProfileId,
                                               CellPhone = p.CellPhone,
                                               City = p.City,
                                               ZipCode = p.ZipCode,
                                               DateOfBirth = p.DateOfBirth,
                                               CreatedOn = p.CreatedOn,
                                               HomePhone = p.HomePhone,
                                               FirstName = p.FirstName,
                                               GenderId = p.FkGenderId,
                                               JobTitle = p.JobTitle,
                                               MiddleInitial = p.MiddleInitial,
                                               State = p.State,
                                               LastName = p.LastName,
                                               StreetAddress = p.StreetAddress,
                                               WorkEmail = p.WorkEmail,
                                               FkInitiatedById = p.FkInitiatedById,
                                               FkUserId = p.FkUserId


                                           }).Single();
                return employeeProfileModel;
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
        public async Task<int> UpdateEmployeeProfile(EmployeeProfileDto requestDto)
        {
            try
            {
                var employeeProfile = new IEmployeeProfile
                {
                    PkEmployeeProfileId = requestDto.EmployeeProfileId,
                    FirstName = requestDto.FirstName,
                    LastName = requestDto.LastName,
                    CellPhone = requestDto.CellPhone,
                    City = requestDto.City,
                    DateOfBirth = requestDto.DateOfBirth,
                    HomePhone = requestDto.HomePhone,
                    MiddleInitial = requestDto.MiddleInitial,
                    JobTitle = requestDto.JobTitle,
                    State = requestDto.State,
                    StreetAddress = requestDto.StreetAddress,
                    FkInitiatedById = requestDto.FkInitiatedById,
                    WorkEmail = requestDto.WorkEmail,
                    ZipCode = requestDto.ZipCode,
                    FkGenderId = requestDto.GenderId,
                    FkUserId = requestDto.FkUserId,
                    UpdatedOn = DateTime.Now,
                };
                _context.EmployeeProfile.Update(employeeProfile);
                _context.SaveChanges();
                return await Task.FromResult(employeeProfile.PkEmployeeProfileId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
