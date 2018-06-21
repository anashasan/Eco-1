using Host.Business.IDbServices;
using Host.Data;
using Host.DataContext;
using Host.Models.AccountViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.DbServices
{
    public class RoleService : IRoleService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EcoDbContext _context;

        public RoleService(IServiceProvider serviceProvider,
                           EcoDbContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }
        
        public void AddUserRole(string userId, string roleId)
        {
            try
            {
                var userRole = new AspNetUserRoles
                {
                    UserId = userId,
                    RoleId = roleId
                };
                _context.AspNetUserRoles.Add(userRole);
                _context.SaveChanges();
                                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<RolesModel> GetAllRoles()
        {
            var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return context.Roles
                           .AsNoTracking()
                           .Select(i => new RolesModel
                           {
                               RoleId = i.Id,
                               Name = i.Name,
                               NormalizedName = i.NormalizedName
                           })
                           .ToList();
        }

        public  List<RolesModel> GetAllRolesById(string userId)
        {
            try
            {
                var roleIds = _context.AspNetUserRoles
                               .AsNoTracking()
                               .Where(p => p.UserId == userId)
                               .Select(i => i.RoleId)
                               .ToList();
                var roles = _context.AspNetRoles
                            .AsNoTracking()
                            .Where(i => roleIds.Contains(i.Id))
                            .Select(p => new RolesModel
                            {
                                 Name = p.Name,
                                 NormalizedName = p.NormalizedName,
                                 RoleId = p.Id
                            }).ToList();
                return  roles;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetRoleByName(string roleName)
        {
            try
            {
                return _context.AspNetRoles
                        .AsNoTracking()
                        .Where(p => p.Name == roleName)
                        .Select(i => i.Id).Single();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetRoleNameById(string userId)
        {
            try
            {
                return _context.AspNetUserRoles
                        .AsNoTracking()
                        .Where(i => i.UserId == userId)
                        .Select(p => p.Role.Name)
                        .Single();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetUserNameByEmail(string email)
        {
            try
            {
                return _context.AspNetUsers
                       .AsNoTracking()
                       .Where(i => i.Email == email)
                       .Select(o => o.UserName)
                       .SingleOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
