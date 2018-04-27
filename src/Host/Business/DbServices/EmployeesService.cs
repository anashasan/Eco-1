using Host.Business.IDbServices;
using Host.DataContext;
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
        public List<UserInfoModel> GetAllEmployee()
        {
            try
            {
              var employee =  _context.AspNetUsers
                       .AsNoTracking()
                       .Select(i => new UserInfoModel
                       {
                           UserName = i.UserName,
                           Email = i.Email,
                           RoleName = i.AspNetUserRoles.Select(p => p.Role.Name).SingleOrDefault(),
                           NormalizeUserName = i.NormalizedUserName,
                       }).ToList();
                return employee;
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
    }
}
