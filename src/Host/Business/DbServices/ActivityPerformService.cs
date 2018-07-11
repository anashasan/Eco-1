using Dapper;
using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<List<GroupActivityReports>> ActivityFilterReport()
        {
            var connection = _context.Database.GetDbConnection();
            var model = (await connection.QueryAsync<ActivityFilterReportsDto>(

                "[dbo].[usp_DailyReport]"
                ,
                commandType: CommandType.StoredProcedure)
               ).ToList();
            var abc = model.GroupBy(x => x.StationName).Select(i => new GroupActivityReports
            {
                StatioName = i.Key,
                DailyReports = i.ToList()
            }).ToList()
            ;
            var modelGroup = model.OrderBy(x => x.StationName)
                       .GroupBy(x => x.StationName).ToList();
            
            return abc;

                                       
        }

        public async Task<List<GroupActivityReports>> ActivityFilterReporByBranchIdt(int branchId, int locationId)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                var model = (await connection.QueryAsync<ActivityFilterReportsDto>(

                    "[dbo].[usp_DailyReport]"
                    ,
                    commandType: CommandType.StoredProcedure)
                   ).ToList();
                var abc = model.GroupBy(x => x.StationName).Select(i => new GroupActivityReports
                {
                    StatioName = i.Key,
                    DailyReports = i.ToList()
                }).ToList()
                ;
                return abc;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> ActivityPerform(ActivityPerformDto requestDto)
        {
            try
            {
                var activityPerform = new ActivityPerform
                {
                    FkStationLocationId = requestDto.StationId,
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
