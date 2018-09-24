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
                var activityPerform = new DataContext.ActivityPerform
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

        public async Task<List<ReportDto>> ActivityReport(int? locationId, DateTime? fromDate, DateTime? toDate, int? branchId)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                var models = (await connection.QueryAsync<DailyActivityPerformReportDto>(
                    "[dbo].[usp_DailyActivityPerformReport]",
                    new
                    {
                        paramLocationId = locationId,
                        paramFromDate = fromDate,
                        paramToDate = toDate,
                        paramBranchId = branchId
                    },
                    commandType: CommandType.StoredProcedure)
                   ).ToList();

                var reportDto = new List<ReportDto>();
                var stationName = string.Empty;
                var perform = 0;
                var dailyActivities = new Stack<DailyActivityPerformReportDto>();
                var activities = new HashSet<string>();
                for (var index = 0; index < models.Count; index++)
                {
                    var model = models[index];
                    if ((stationName != model.StationName && !string.IsNullOrEmpty(stationName)) || index == models.Count - 1)
                    {
                        if (index == models.Count - 1)
                        {
                            activities.Add(model.ActivityName);

                            stationName = model.StationName;

                            dailyActivities.Push(model);
                        }
                        var dailyReportActivities = new List<DailyActivityPerformReportDto>(dailyActivities.Count);
                        var stationNo = 0;
                        var locationName = string.Empty;
                        var activityName = string.Empty;
                        var activityPerformance = new Stack<ActivityPerformance>();

                        while (dailyActivities.Count != 0)
                        {
                            var dailyActivityPerform = dailyActivities.Pop();
                            if ((stationNo != dailyActivityPerform.StationNo && stationNo != 0 &&
                                 locationName != dailyActivityPerform.LocationName &&
                                 !string.IsNullOrEmpty(locationName)) ||
                                dailyActivities.Count == 0)
                            {
                                var stationNoActivities = new List<ActivityPerformance>(activityPerformance.Count);
                                while (activityPerformance.Count != 0)
                                {
                                    stationNoActivities.Add(activityPerformance.Pop());
                                }

                                dailyReportActivities.Add(new DailyActivityPerformReportDto
                                {
                                    StationName = stationName,
                                    LocationName = locationName,
                                    StationNo = stationNo,
                                    ActivityPerform = stationNoActivities
                                });
                            }
                            else if (stationNo == dailyActivityPerform.StationNo &&
                                     locationName == dailyActivityPerform.LocationName &&
                                     activityName != dailyActivityPerform.ActivityName &&
                                     !string.IsNullOrEmpty(activityName))
                            {
                                activityPerformance.Push(new ActivityPerformance
                                {
                                    ActivityName = activityName,
                                    Perform = perform
                                });
                                perform = 0;
                            }

                            stationNo = dailyActivityPerform.StationNo;
                            locationName = dailyActivityPerform.LocationName;
                            perform = dailyActivityPerform.Perform + perform;
                            activityName = dailyActivityPerform.ActivityName;
                        }

                        reportDto.Add(new ReportDto
                        {
                            StationName = stationName,
                            DailyActivityPerformReport = dailyReportActivities,
                            Activities = activities.Any() ? activities.ToList() : null,
                        });
                        if (activities.Any())
                            activities.Clear();
                    }

                    activities.Add(model.ActivityName);

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
        //public List<GraphActivityPerform> StationReport()
        //{
        //    var connection = _context.Database.GetDbConnection();

        //    var models = connection.Query<StationReportDto>
        //        ("[dbo].[usp_Graphreport]", commandType: CommandType.StoredProcedure)
        //          .ToList();
        //    var stationreport = new List<GraphActivityPerform>();
        //    var dailyactivitiesperform = new Stack<MonthlyPerform>();
        //    var activityName = string.Empty;

        //    foreach (var model in models)
        //    {
        //        if (activityName != model.Activity && !string.IsNullOrEmpty(activityName))
        //        {
        //            var monthlyPerform = new List<MonthlyPerform>(dailyactivitiesperform.Count);
        //            while (dailyactivitiesperform.Count != 0)
        //            {
        //                monthlyPerform.Add(dailyactivitiesperform.Pop());
        //            }

        //            stationreport.Add(new GraphActivityPerform
        //            {
        //                Activity = activityName,
        //                MonthlyPerform = monthlyPerform
        //            });
        //        }

        //        activityName = model.Activity;
        //        dailyactivitiesperform.Push(new MonthlyPerform
        //        {
        //            Month = model.Month,
        //            Value = model.ActivityType == "Input" ? model.Perform : model.isperform,
        //        });
        //    }

        //    return stationreport;
        //}

        //public List<GraphActivityPerform> StationReport()
        //{
        //    var connection = _context.Database.GetDbConnection();

        //    var models = connection.Query<StationReportDto>
        //        ("[dbo].[usp_Graphreport]", commandType: CommandType.StoredProcedure)
        //          .ToList();
        //    var station = new List<StationActivityDto>();
        //    var stationreport = new List<GraphActivityPerform>();
        //    var dailystationactivity = new Stack<GraphActivityPerform>();
        //    var dailyactivitiesperform = new Stack<MonthlyPerform>();
        //    var activityName = string.Empty;

        //    foreach (var model in models)
        //    {
        //        if (activityName != model.Activity && !string.IsNullOrEmpty(activityName))
        //        {
        //            var monthlyPerform = new List<MonthlyPerform>(dailyactivitiesperform.Count);
        //            while (dailyactivitiesperform.Count != 0)
        //            {
        //                monthlyPerform.Add(dailyactivitiesperform.Pop());
        //            }

        //            stationreport.Add(new GraphActivityPerform
        //            {
        //                Activity = activityName,
        //                MonthlyPerform = monthlyPerform
        //            });
        //        }

        //        activityName = model.Activity;
        //        dailyactivitiesperform.Push(new MonthlyPerform
        //        {
        //            Month = model.Month,
        //            Value = model.ActivityType == "Input" ? model.Perform : model.isperform,
        //        });
        //    }

        //    return stationreport;
        //}


    }

}


