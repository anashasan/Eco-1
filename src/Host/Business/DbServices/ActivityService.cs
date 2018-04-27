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
    public class ActivityService : IActivityService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly EcoDbContext _context;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ActivityService(EcoDbContext context)
        {
            _context = context;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> AddActivity(StationActivityDto requestDto)
        {
            try
            {
                var activities = requestDto.Activities.Select(i => new Activity
                {
                    Name = i.Name,
                    Description = i.Description,
                    CreateOn = DateTime.Now
                });

                _context.Activity.AddRange(activities);
                _context.SaveChanges();
                _context.StationActivity.AddRange(activities.Select(p => new StationActivity
                {
                    FkActivityId = p.PkActivityId,
                    FkStationId = requestDto.StationId
                }));
                return await Task.FromResult(_context.SaveChanges());
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
        public ActivityDto GetActivityById(int id)
        {
            return _context.Activity
                   .AsNoTracking()
                   .Where(i => i.PkActivityId == id)
                   .Select(p => new ActivityDto
                   {
                      ActivityId = p.PkActivityId,
                      Name = p.Name,
                      Description = p.Description
                   }).Single();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ActivityDto> GetAllActivity()
        {
            return _context.Activity
                   .AsNoTracking()
                   .Select(p => new ActivityDto
                   {
                       ActivityId = p.PkActivityId,
                       Name = p.Name,
                       Description = p.Description
                   }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> UpdateActivity(ActivityDto requestDto)
        {
            try
            {
                var activity = new Activity
                {
                    Name = requestDto.Name,
                    Description = requestDto.Description,
                    UpdatedOn = DateTime.Now
                };
                _context.Activity.Update(activity);
                _context.SaveChanges();
                return await Task.FromResult(activity.PkActivityId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
