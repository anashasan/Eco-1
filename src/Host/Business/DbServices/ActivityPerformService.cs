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
using System.Globalization;

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
                //var date = requestDto.Activities.Select(i => i.ActivityDateTime).FirstOrDefault();
                //var createdOn = DateTime.Parse(date);
                //var date = requestDto.ActivityDateTime.ToString();
                //DateTime parsed = DateTime.ParseExact(requestDto.ActivityDateTime,
                //                      "ddd, dd MMMM yyyy HH:mm:ss Z",
                //                       CultureInfo.InvariantCulture);
                //DateTime dateTime = ISODateTimeFormat.dateTimeParser().parseDateTime(timestamp);
                string date = requestDto.ActivityDateTime;
                var activityObservations = new List<ActivityObservation>();
                var activityPerform = new DataContext.ActivityPerform


                {
                    FkStationLocationId = requestDto.StationLocationId,
                    FkEmployeeId = requestDto.EmployeeId,
                    //CreatedOn = new DateTime(requestDto.ActivityDateTime.FirstOrDefault())
                    //CreatedOn = DateTime.ParseExact(requestDto.ActivityDateTime, "M/d/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                    CreatedOn = DateTime.Parse(date)

                };
                _context.ActivityPerform.Add(activityPerform);
                _context.SaveChanges();

                var lstActivityPerformDetail = new List<ActivityPerformDetail>();
                foreach (var activity in requestDto.Activities)
                {
                    if (activity.Observation != null && activity.Observation.Any())
                    {
                        for (int i = 0; i < activity.Observation.Count; i++)
                        {
                            var image = activity.Images[i];
                            var description = activity.Observation[i];
                            activityObservations.Add(new ActivityObservation
                            {
                                Images = image,
                                Description = description,

                            });

                        }
                        // byte j = 0;
                        lstActivityPerformDetail.Add(new ActivityPerformDetail
                        {

                            FkActivityId = activity.ActivityId,
                            FkActivityPerformId = activityPerform.PkActivityPerformId,
                            //CreatedOn = createdOn,
                            ActivityObservation = activityObservations,
                            CreatedOn = DateTime.Parse(date)
                            //CreatedOn = new DateTime(requestDto.ActivityDateTime.FirstOrDefault())
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
                            CreatedOn = DateTime.Parse(date)
                            //CreatedOn = new DateTime(requestDto.ActivityDateTime.FirstOrDefault())
                            //CreatedOn = DateTime.Now
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

        //public async Task<int> ActivityPerform(ActivityPerformDto requestDto)
        //{
        //    try
        //    {


        //        var activityPerform = new DataContext.ActivityPerform
        //        {
        //            FkStationLocationId = requestDto.StationLocationId,
        //            FkEmployeeId = requestDto.EmployeeId,
        //            CreatedOn = DateTime.Now,

        //        };
        //        _context.ActivityPerform.Add(activityPerform);
        //        _context.SaveChanges();

        //        var lstActivityPerformDetail = new List<ActivityPerformDetail>();
        //        foreach (var activity in requestDto.Activities)
        //        {
        //            if (activity.Observation != null && activity.Observation.Any())
        //            {
        //                //  byte j = 0;
        //                lstActivityPerformDetail.Add(new ActivityPerformDetail
        //                {

        //                    FkActivityId = activity.ActivityId,
        //                    FkActivityPerformId = activityPerform.PkActivityPerformId,
        //                    CreatedOn = DateTime.Now,
        //                    ActivityObservation = activity.Observation.Select(i => new ActivityObservation
        //                    {
        //                        Description = i
        //                    })

        //                    .ToList(),

        //                });
        //            }


        //            else
        //            {
        //                lstActivityPerformDetail.Add(new ActivityPerformDetail
        //                {
        //                    FkActivityId = activity.ActivityId,
        //                    FkActivityPerformId = activityPerform.PkActivityPerformId,
        //                    IsPerform = activity.IsPerform,
        //                    Perform = activity.Perform,
        //                    CreatedOn = DateTime.Now,
        //                });
        //            }
        //        }

        //        _context.ActivityPerformDetail.AddRange(lstActivityPerformDetail);

        //        return await Task.FromResult(_context.SaveChanges());


        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        //public async Task<int> ActivityPerform1(ActivityPerformDto1 requestDto1)
        //{
        //    try
        //    {
        //        var dis = new List<IDictionary<int, string>>();  
        //        List<ActivityObservation> c = new List<ActivityObservation>();
        //        c.Add(new ActivityObservation
        //        {
        //            req
        //        })
        //        List<string> observation;
        //        List<string> images;
        //        var a = new ActivityObservation();
        //        var model = requestDto1.Activities.Select(i => new {i.Images, i.Observation }
        //        { 
        //            = i.Observation,
        //           images = i.Images
        //        });
        //        foreach (var item in model)
        //        {
        //            foreach (var i in item.images)
        //            {
        //                string c = i;
        //            }
        //        }
        //        model.Select(i => new ActivityObservation
        //        {
        //            Images = i.images
        //        })

        //         var activityPerform = new DataContext.ActivityPerform
        //        {
        //            FkStationLocationId = requestDto1.StationLocationId,
        //            FkEmployeeId = requestDto1.EmployeeId,
        //            CreatedOn = DateTime.Now,

        //        };
        //        _context.ActivityPerform.Add(activityPerform);
        //        _context.SaveChanges();

        //        var lstActivityPerformDetail = new List<ActivityPerformDetail>();
        //        foreach (var activity in requestDto1.Activities)
        //        {
        //            if (activity.Observation != null && activity.Observation.Any())
        //            {
        //                //  byte j = 0;
        //                lstActivityPerformDetail.Add(new ActivityPerformDetail
        //                {

        //                    FkActivityId = activity.ActivityId,
        //                    FkActivityPerformId = activityPerform.PkActivityPerformId,
        //                    CreatedOn = DateTime.Now,
        //                    ActivityObservation = model
        //                });
        //            }
        //            else
        //            {
        //                lstActivityPerformDetail.Add(new ActivityPerformDetail
        //                {
        //                    FkActivityId = activity.ActivityId,
        //                    FkActivityPerformId = activityPerform.PkActivityPerformId,
        //                    IsPerform = activity.IsPerform,
        //                    Perform = activity.Perform,
        //                    CreatedOn = DateTime.Now,
        //                });
        //            }
        //        }

        //        _context.ActivityPerformDetail.AddRange(lstActivityPerformDetail);

        //        return await Task.FromResult(_context.SaveChanges());


        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

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
                var stationNo = 0;
                var locationName = string.Empty;
                var activityName = string.Empty;
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
                        var activityPerformance = new Stack<ActivityPerformance>();

                        while (dailyActivities.Count != 0)
                        {
                            var dailyActivityPerform = dailyActivities.Pop();
                            if (stationNo != dailyActivityPerform.StationNo && stationNo != 0 &&
                                !string.IsNullOrEmpty(locationName) ||
                                dailyActivities.Count == 0)
                            {
                                if (dailyActivities.Count == 0)
                                    activityPerformance.Push(new ActivityPerformance
                                    {
                                        ActivityName = dailyActivityPerform.ActivityName,
                                        Perform = dailyActivityPerform.Perform
                                    });

                                activityPerformance.Push(new ActivityPerformance
                                {
                                    ActivityName = activityName,
                                    Perform = perform
                                });
                                var stationNoActivities = new List<ActivityPerformance>(activityPerformance.Count);
                                while (activityPerformance.Count != 0)
                                    stationNoActivities.Add(activityPerformance.Pop());

                                dailyReportActivities.Add(new DailyActivityPerformReportDto
                                {
                                    StationName = stationName,
                                    LocationName = locationName,
                                    StationNo = stationNo,
                                    ActivityPerform = stationNoActivities
                                });
                                perform = 0;
                            }
                            else if ((stationNo == dailyActivityPerform.StationNo &&
                                     activityName != dailyActivityPerform.ActivityName &&
                                     !string.IsNullOrEmpty(activityName)) || dailyActivityPerform.LocationName != locationName)
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

                    perform = stationNo = 0;
                    activityName = locationName = string.Empty;

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
        public List<Graph> StationReport(int? locationId, DateTime? fromDate, DateTime? toDate, int? branchId)
        {
            var connection = _context.Database.GetDbConnection();

            var models = connection.Query<StationReportDto>
                ("[dbo].[usp_DailyGraphReport]",
                 new
                 {
                     @paramLocationId = locationId,
                     @paramFromDate = fromDate,
                     @paramToDate = toDate,
                     @paramBranchId = branchId
                 },
                commandType: CommandType.StoredProcedure)
               .ToList();


            //var graph = new List<Graph>();
            //var station = new List<GraphActivityPerform>();
            //var activities = new List<Activities>();

            //var dailyactivitiesperform = new Stack<MonthlyPerform>();
            //var activityName = string.Empty;
            //var locationname = string.Empty;
            //var stationname = string.Empty;

            var data = models
                .GroupBy(i => i.LocationName)
                .Select(g => new Graph
                {
                    LocationName = g.Key,
                    Stations = g
                        .GroupBy(f => f.StationName)
                        .Select(i => new GraphActivityPerform
                        {
                            StationName = i.Key,
                            Activity = i
                                .GroupBy(o => o.ActivityName)
                                .Select(j => new Activities
                                {
                                    ActivityName = j.Key,
                                    MonthlyPerform = j
                                        .GroupBy(z => z.Month)
                                        .Select(z => new MonthlyPerform
                                        {
                                            Month = int.Parse(z.Key),
                                            Perform = z.Sum(X => X.Perform)
                                        })
                                        .ToList()
                                })
                                .ToList()
                        })
                    .ToList()
                })
                .ToList();

            foreach (var model in data)
            {

                foreach (var stations in model.Stations)
                {
                    foreach (var activities in stations.Activity)
                    {
                        foreach (var monthNumber in Enumerable.Range(1, 12))
                        {
                            if (!activities.MonthlyPerform.Any() ||
                                activities.MonthlyPerform.All(i => i.Month != monthNumber))
                            {
                                activities.MonthlyPerform.Add(new MonthlyPerform
                                {
                                    Month = monthNumber,
                                    Perform = 0
                                });
                            }
                        }
                        activities.MonthlyPerform = activities.MonthlyPerform
                            .OrderBy(i => i.Month)
                            .ToList();

                    }
                }
            }


            return data;
            //{
            //    var model = models[i];

            //    if (locationname != model.LocationName && !string.IsNullOrEmpty(locationname))
            //    {
            //        graph.Add(new Graph
            //        {
            //            LocationName = locationname,
            //            Stations = station,
            //        });

            //        station = null;
            //        station = new List<GraphActivityPerform>();
            //    }

            //    if (stationname != model.StationName && !string.IsNullOrEmpty(stationname))
            //    {
            //        station.Add(new GraphActivityPerform
            //        {
            //            StationName = stationname,
            //            Activity = activities
            //        });

            //        activities = null;
            //        activities = new List<Activities>();
            //    }

            //    if (activityName != model.ActivityName && !string.IsNullOrEmpty(activityName))
            //    {
            //        var monthlyPerform = new List<MonthlyPerform>(dailyactivitiesperform.Count);
            //        while (dailyactivitiesperform.Count != 0)
            //        {
            //            monthlyPerform.Add(dailyactivitiesperform.Pop());
            //        }

            //        activities.Add(new Activities
            //        {

            //            ActivityName = activityName,
            //            MonthlyPerform = monthlyPerform
            //        });


            //    }

            //    dailyactivitiesperform.Push(new MonthlyPerform
            //    {
            //        Month = model.Month,
            //        Perform = model.Perform
            //    });
            //    locationname = model.LocationName;
            //    activityName = model.ActivityName;
            //    stationname = model.StationName;
            //}

            //return graph;
        }

        public List<GetDailyReportDto> GetDailyReport()
        {
            throw new NotImplementedException();
        }


        //        public List<GraphActivityPerform> StationReport(int? locationId, DateTime? fromDate, DateTime? toDate, int? branchId)
        //        {
        //            var connection = _context.Database.GetDbConnection();

        //            var models = connection.Query<StationReportDto>
        //                ("[dbo].[usp_DailyGraphReport]",
        //                 new
        //                 {
        //                     @paramLocationId = locationId,
        //                     @paramFromDate = fromDate,
        //                     @paramToDate = toDate,
        //                     @paramBranchId = branchId
        //                 },
        //                commandType: CommandType.StoredProcedure)
        //               .ToList();


        //            var stationreport = new List<GraphActivityPerform>();
        //        var dailyactivitiesperform = new Stack<MonthlyPerform>();
        //        var activityName = string.Empty;

        //            foreach (var model in models)
        //            {
        //                if (activityName != model.ActivityName && !string.IsNullOrEmpty(activityName))
        //                {
        //                    var monthlyPerform = new List<MonthlyPerform>(dailyactivitiesperform.Count);
        //                    while (dailyactivitiesperform.Count != 0)
        //                    {
        //                        monthlyPerform.Add(dailyactivitiesperform.Pop());
        //                    }

        //    stationreport.Add(new GraphActivityPerform
        //                    {
        //                        Activity = activityName,
        //                        MonthlyPerform = monthlyPerform
        //});
        //                }

        //                activityName = model.ActivityName;
        //                dailyactivitiesperform.Push(new MonthlyPerform
        //                {
        //                    Month = model.Month,
        //                    Value = model.ActivityType == "Input" ? model.Perform : model.isperform,
        //                });
        //            }

        //            return stationreport;
        //        }

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



       
        public List<GetDailyReportDto> DailyReport()
        {
            try
            {
                return _context.ActivityPerformDetail
                        .AsNoTracking()
                        .Select(i => new GetDailyReportDto
                        {
                            Activity=i.FkActivity.Name,
                            Perform = i.Perform,
                            IsPerform = i.IsPerform,
                            ActivityDate=i.CreatedOn
                          
                            


                        })
                        .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
       public void UpdateDailyReport(GetDailyReportDto requestDto)
        {
            var model = new ActivityPerformDetail { PkActivityPerformDetailId = requestDto.ActivityPerformDetailId };
            _context.ActivityPerformDetail.Attach(model);
            model.Perform = requestDto.Perform;
            model.IsPerform = requestDto.IsPerform;
            _context.SaveChanges();
            
                
        }
    }
}




