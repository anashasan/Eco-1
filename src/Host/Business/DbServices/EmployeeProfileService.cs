using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Host.Models;
using Microsoft.AspNetCore.Identity;
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
                var employeeProfile = new EmployeeProfile
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


                                           }).SingleOrDefault();
                return employeeProfileModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool IsEmailExist(string email)
        {
            try
            {
                return _context.AspNetUsers
                       .AsNoTracking()
                       .Any(i => i.Email == email);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool IsUserNameExist(string userName)
        {
            try
            {
                return _context.AspNetUsers
                      .AsNoTracking()
                      .Any(i => i.UserName == userName);
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
                var employeeModel = _context.EmployeeProfile.Find(requestDto.EmployeeProfileId);

                employeeModel.LastName = requestDto.LastName;
                employeeModel.CellPhone = requestDto.CellPhone;
                employeeModel.City = requestDto.City;
                employeeModel.HomePhone = requestDto.HomePhone;
                employeeModel.MiddleInitial = requestDto.MiddleInitial;
                employeeModel.JobTitle = requestDto.JobTitle;
                employeeModel.StreetAddress = requestDto.StreetAddress;
                employeeModel.UpdatedOn = DateTime.Now;
                
               
                _context.EmployeeProfile.Update(employeeModel);
                _context.SaveChanges();
                return await Task.FromResult(employeeModel.PkEmployeeProfileId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
