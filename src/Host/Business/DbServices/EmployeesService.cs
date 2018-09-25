using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Host.Models;
using Host.Models.AccountViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.DbServices
{
    public class EmployeesService : IEmployeesService
    {
        private readonly EcoDbContext _context;
        
        public EmployeesService(EcoDbContext context)
        {
            _context = context;
        }

        public bool CheckEmailIsExist(string email)
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

        public List<UserInfoModel> GetAllEmployee()
        {
            try
            {
                var employee = _context.AspNetUsers
                         .AsNoTracking()
                         .Where(i => i.Status.HasValue && i.Status.Value)
                         .Select(i => new UserInfoModel
                         {
                             Id = i.Id,
                             UserName = i.UserName,
                             Email = i.Email,
                             RoleName = i.AspNetUserRoles.Select(p => p.Role.Name).SingleOrDefault(),
                             NormalizeUserName = i.NormalizedUserName,
                             RoleId = i.AspNetUserRoles.Select(p => p.RoleId).SingleOrDefault()
                       }).ToList();
                return employee;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<UserInfoModel> GetAllInActiveUser()
        {
            try
            {
                var inActiveEmployee = _context.AspNetUsers
                         .AsNoTracking()
                         .Where(i => i.Status.Value== false)
                         .Select(i => new UserInfoModel
                         {
                             Id = i.Id,
                             UserName = i.UserName,
                             Email = i.Email,
                             RoleName = i.AspNetUserRoles.Select(p => p.Role.Name).SingleOrDefault(),
                             NormalizeUserName = i.NormalizedUserName,
                             RoleId = i.AspNetUserRoles.Select(p => p.RoleId).SingleOrDefault()
                         }).ToList();
                return inActiveEmployee;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetUserName(string userId)
        {
            try
            {
                return _context.AspNetUsers
                       .AsNoTracking()
                       .Where(i => i.Id == userId)
                       .Select(p => p.NormalizedUserName)
                       .Single();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void UserActive(string userId)
        {
            try
            {
                var userModel = new AspNetUsers() { Id = userId };
                _context.AspNetUsers.Attach(userModel);
                userModel.Status = true;

                _context.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void UserInActive(string userId)
        {
            try
            {
                var userModel = new AspNetUsers() { Id = userId };
                _context.AspNetUsers.Attach(userModel);
                userModel.Status = false;

                _context.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
