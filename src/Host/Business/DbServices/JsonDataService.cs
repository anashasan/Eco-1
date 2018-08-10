using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Business.DbServices
{
    public class JsonDataService : IJsonDataService
    {
        private readonly EcoDbContext _context;

        public JsonDataService(EcoDbContext context)
        {
            _context = context;
        }

        public JsonDataDto GetJsonData(Guid code, out string message)
        {
            message = "";
           if(Guid.Empty != code)
            {
                if (_context.JsonData
                    .AsNoTracking()
                    .Any(i => i.GeneratedCode == code))
                {
                    message = "Already Updated data you have";
                    return null;
                }

                    return _context.JsonData
                          .AsNoTracking()
                          .Where(i => i.GeneratedCode != code)
                          .Select(i => new JsonDataDto
                          {
                              GeneratedCode = i.GeneratedCode,
                              JsonObject = i.JsonData1
                          }).SingleOrDefault();                
            }
           else
            {
                var josndata = _context.JsonData
                          .AsNoTracking()
                          .Select(i => new JsonDataDto
                          {
                              GeneratedCode = i.GeneratedCode,
                              JsonObject = i.JsonData1
                          }).SingleOrDefault();
                return josndata;
            }
          
        }
    }
}
