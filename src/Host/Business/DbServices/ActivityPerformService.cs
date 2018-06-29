using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.DbServices
{
    public class ActivityPerformService : IActivityPerformService
    {
        private readonly EcoDbContext _context;

        public ActivityPerformService(EcoDbContext context)
        {
            _context = context;
        }

        public async Task<int> ActivityPerform(ActivityPerformDto requestDto)
        {
            try
            {
                var activityPerform = new ActivityPerform
                {
                    FkStationId = requestDto.StationId,
                    FkEmployeeId = requestDto.EmployeeId,
                    CreatedOn = DateTime.Now,
                   
            };
                _context.ActivityPerform.Add(activityPerform);
                _context.SaveChanges();

                var lstActivityPerformDetail = new List<ActivityPerformDetail>();
                foreach (var activity in requestDto.Activities)
                {
                    if(activity.Observations != null && activity.Observations.Any())
                    {
                        lstActivityPerformDetail.Add(new ActivityPerformDetail
                        {
                            FkActivityId = activity.ActivityId,
                            FkActivityPerformId = activityPerform.PkActivityPerformId,
                            CreatedOn = DateTime.Now,
                            ActivityObservation = activity.Observations.Select(i => new ActivityObservation
                            {
                                Description = i,                                
                            })
                            .ToList()
                        });
                    }
                    else
                    {
                        lstActivityPerformDetail.Add(new ActivityPerformDetail
                        {
                            FkActivityId = activity.ActivityId,
                            FkActivityPerformId = activityPerform.PkActivityPerformId,
                            IsPerform = activity.IsPerform,
                            Perform = activity.Perform,
                            CreatedOn = DateTime.Now,
                        });
                    }
                }

                _context.ActivityPerformDetail.AddRange(lstActivityPerformDetail);

                return await Task.FromResult(_context.SaveChanges());
                
                                      
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
