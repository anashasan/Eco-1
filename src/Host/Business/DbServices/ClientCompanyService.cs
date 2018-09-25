using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Business.DbServices
{
    public class ClientCompanyService : IClientCompanyService
    {
        private readonly EcoDbContext _context;

        public ClientCompanyService(EcoDbContext context)
        {
            _context = context;
        }

        public void AddClientCompany(ClientComanyDto dto )
        {
            var model = new ClientCompany
            {
                FkCompanyId = dto.CompanyId,
                FkEmployeeId = dto.UserId,
                CreatedOn = DateTime.Now
            };

            _context.ClientCompany.Add(model);
            _context.SaveChanges();
        }

        public List<ClientComanyDto> GetAllClientCompany()
        {
            var model = (from users in _context.AspNetUsers
                         join clientComp in _context.ClientCompany on users.Id equals clientComp.FkEmployeeId
                         join compId in _context.Company on clientComp.FkCompanyId equals compId.PkCompanyId
                         select new ClientComanyDto
                         { 
                             ClientCompanyId = clientComp.PkClientCompanyId,
                             UserId = users.Id,
                             ClientName = users.UserName,
                             CompanyId = compId.PkCompanyId,
                             CompanyName = compId.Name 

                         }).ToList();

            return model;
        }

        public void UpdateClientCompany(ClientComanyDto dto)
        {
            var exiting = _context.ClientCompany.Find(dto.ClientCompanyId);

            exiting.FkCompanyId = dto.CompanyId;
            exiting.FkEmployeeId = dto.UserId;

            _context.ClientCompany.Update(exiting);
            _context.SaveChanges();
        }
    }
}
