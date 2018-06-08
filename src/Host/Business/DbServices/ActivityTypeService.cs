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
    public class ActivityTypeService : IActivityTypeService
    {
        private readonly EcoDbContext _context;

        public ActivityTypeService(EcoDbContext context)
        {
            _context = context;
        }


        public List<ActivityTypeDto> GetAllActivityType()
        {
            try
            {
                return _context.ActivityType
                       .AsNoTracking()
                       .Select(i => new ActivityTypeDto
                       {
                           ActivityTypeId = i.PkActivityTypeId,
                           Type = i.Type
                       }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
