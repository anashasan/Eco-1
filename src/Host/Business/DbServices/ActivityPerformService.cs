using Dapper;
using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
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
                    FkStationLocationId = requestDto.StationLocationId,
                    FkEmployeeId = requestDto.EmployeeId,
                    CreatedOn = DateTime.Now,

                };
                _context.ActivityPerform.Add(activityPerform);
                _context.SaveChanges();

                var lstActivityPerformDetail = new List<ActivityPerformDetail>();
                foreach (var activity in requestDto.Activities)
                {
                    if (activity.Observation != null && activity.Observation.Any())
                    {
                        lstActivityPerformDetail.Add(new ActivityPerformDetail
                        {
                            FkActivityId = activity.ActivityId,
                            FkActivityPerformId = activityPerform.PkActivityPerformId,
                            CreatedOn = DateTime.Now,
                            ActivityObservation = activity.Observation.Select(i => new ActivityObservation
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

        public async Task<List<ReportDto>> ActivityReport(int? locationId, DateTime? createdOn)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                var models = (await connection.QueryAsync<DailyActivityPerformReportDto>(
                    "[dbo].[usp_DailyActivityPerformReport]",
                    new { paramLocationId = locationId, paramDate = createdOn },
                    commandType: CommandType.StoredProcedure)
                   ).ToList();

                var reportDto = new List<ReportDto>();
                var stationName = string.Empty;
                var dailyActivities = new Stack<DailyActivityPerformReportDto>();
                var activities = new Stack<string>();
                foreach (var model in models)
                {
                    if (stationName != model.StationName && !string.IsNullOrEmpty(stationName))
                    {
                        var reportActivities = new List<string>(activities.Count);
                        while (activities.Count != 0)
                        {
                            reportActivities.Add(activities.Pop());
                        }

                        var dailyReportActivities = new List<DailyActivityPerformReportDto>(dailyActivities.Count);
                        while (dailyActivities.Count != 0)
                        {
                            dailyReportActivities.Add(dailyActivities.Pop());
                        }

                        reportDto.Add(new ReportDto
                        {
                            StationName = stationName,
                            DailyActivityPerformReport = dailyReportActivities,
                            Activities = reportActivities.Any() ? reportActivities : null,
                        });
                    }

                    if (model.ActivityPerformJson != null && model.ActivityPerformJson.Any())
                    {
                        model.ActivityPerform = JsonConvert.DeserializeObject<List<DailyActivityPerformDetailDto>>(model.ActivityPerformJson);
                        if (model.ActivityPerform != null && model.ActivityPerform.Any())
                        {
                            foreach (var activityPerform in model.ActivityPerform)
                            {
                                if (!string.IsNullOrEmpty(activityPerform.ActivityName))
                                {
                                    if (!activities.Contains(activityPerform.ActivityName))
                                        activities.Push(activityPerform.ActivityName);
                                }
                            }
                        }
                    }
                    stationName = model.StationName;
                    dailyActivities.Push(model);


                }

                return reportDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public List<StationReportDto> StationReport()
        {
            var connection = _context.Database.GetDbConnection();

            return connection.Query<StationReportDto>("[dbo].[usp_Graphreport]", commandType: CommandType.StoredProcedure)
                  .ToList();


        }

    }

}


