﻿using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;

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

        public async Task<int> AddActivity(ActivityDto requestDto)
        {
            try
            {
                var activity = new Activity
                {

                    Name = requestDto.Name,
                    Description = requestDto.Description,
                    FkActivityTypeId = requestDto.ActivityTypeId

                };


                _context.Activity.Add(activity);
                _context.SaveChanges();

                var stationactivity = new StationActivity
                {
                    FkStationId = requestDto.StationId,
                    FkActivityId = activity.PkActivityId
                };

                _context.StationActivity.Add(stationactivity);
                return await Task.FromResult(_context.SaveChanges());



            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public int DeleteActivityById(int? id)
        //{
        //    var listOfActivityIds = _context.StationActivity.Find(id);
        //    _context.StationActivity.Remove(listOfActivityIds);
        //    _context.SaveChanges();
        //    return listOfActivityIds.PkStationActivityId;
        //}
            {
            var connection = _context.Database.GetDbConnection();
      var a=  connection.Execute(
                       "[dbo].[usp_DeleteActivty]"
                       , new { @paramActivityId = id
                          

                       }
                       , commandType: CommandType.StoredProcedure);

           return a;
        }

/// <summary>
/// 
/// </summary>
/// <param name="requestDto"></param>
/// <returns></returns>
/*  public async Task<int> AddActivity(StationActivityDto requestDto)
  {
      try
      {
          var activities = requestDto.Activities.Select(i => new Activity
          {
              PkActivityId = i.ActivityId,
              Name = i.Name,
              Description = i.Description,
              CreateOn = DateTime.Now,
              StationActivity = new List<StationActivity>
              {
                  new StationActivity
                  {
                      FkStationId = requestDto.StationId
                  }
              }
          });

          _context.Activity.AddRange(activities);
         await _context.SaveChangesAsync();
          return await Task.FromResult(_context.SaveChanges());
      }
      catch (Exception e)
      {
          Console.WriteLine(e);
          throw;
      }
  }
  */
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
                      Description = p.Description,
                      ActivityTypeId = p.FkActivityTypeId,
                      Type = p.FkActivityType.Type
                   }).Single();
        }

        /*  public GetStationActivityDto GetActivityByStationId(int id)
          {
              try
              {
                  var activities = _context.Station
                                   .AsNoTracking()
                                   .Where(i => i.PkStationId == id)
                                   .Select(p => new GetStationActivityDto
                                   {
                                       StationId = p.PkStationId,
                                       StationName = p.Name,
                                       Activities = p.StationActivity.Select(i => new ActivityDto
                                       {
                                           ActivityId = i.FkActivityId,
                                           Description = i.FkActivity.Description,
                                           Name = i.FkActivity.Name
                                       }).ToList()
                                   }).Single();
                  return activities;
              }
              catch (Exception e)
              {
                  Console.WriteLine(e);
                  throw;
              }
          }*/

       public List<ActivityDto> GetActivityByStationId(int id)
        {
            try
            {
                var stationactivity = _context.StationActivity
                    .AsNoTracking()
                    .Where(i => i.FkStationId == id)
                    .Select(a => new ActivityDto
                    {
                        ActivityId = a.FkActivity.PkActivityId,
                        Name = a.FkActivity.Name,
                        Description = a.FkActivity.Description,
                        Type = a.FkActivity.FkActivityType.Type
                    }).ToList();
                return stationactivity;
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
        /// <returns></returns>
        public List<ActivityDto> GetAllActivity()
        {
            return _context.Activity
                   .AsNoTracking()
                   .Select(p => new ActivityDto
                   {
                       ActivityId = p.PkActivityId,
                       Name = p.Name,
                       Description = p.Description,
                       ActivityTypeId = p.FkActivityTypeId,
                       Type = p.FkActivityType.Type
                   }).ToList();
        }

        public List<ObservationReportDto> GetObservationReport(int branchId, int? locationId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var observationReport = (from activityObservation in _context.ActivityObservation
                                         join activitPerformDetail in _context.ActivityPerformDetail on activityObservation.FkActivityPerformDetailId equals activitPerformDetail.PkActivityPerformDetailId
                                         join activityPerform in _context.ActivityPerform on activitPerformDetail.FkActivityPerformId equals activityPerform.PkActivityPerformId
                                         join stationLocation in _context.StationLocation on activityPerform.FkStationLocationId equals stationLocation.PkStationLocationId
                                         join station in _context.Station on stationLocation.FkStationId equals station.PkStationId
                                         join branchLocation in _context.BranchLocation on stationLocation.FkLocationId equals branchLocation.FkLocationId
                                         join location in _context.Location on branchLocation.FkLocationId equals location.PkLocationId
                                         where branchLocation.FkBranchId == branchId &
                                               (!locationId.HasValue || branchLocation.FkLocationId == locationId ) &
                                               ((!fromDate.HasValue & !toDate.HasValue) || activitPerformDetail.CreatedOn >= fromDate && activitPerformDetail.CreatedOn <= toDate)
                                         select new ObservationReportDto
                                         {
                                             ActivityObservationId = activityObservation.PkActivityObservationId,
                                             BranchId = branchId,
                                             BranchName=branchLocation.FkBranch.Name,
                                             CompanyName=branchLocation.FkBranch.CompanyBranch.Select(i=>i.FkCompany.Name).SingleOrDefault(),
                                             ClientReview = activityObservation.ClientReview,
                                             ClientReviewDate = activityObservation.ClinetReviewDate,
                                             Description = activityObservation.Description,
                                             LocationName = location.Name,
                                             LocationId = location.PkLocationId,
                                             ObervationNumber = 0,
                                             StationId = station.PkStationId,
                                             StationName= station.Name,
                                             StationNumber = stationLocation.Sno,
                                             Status = activityObservation.Status,
                                             Images = activityObservation.Images,
                                             ObservationDate = activitPerformDetail.CreatedOn.ToShortDateString()
                                             
                                         }).ToList();

                return observationReport;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        public async Task<int> UpdateActivity(ActivityDto requestDto)
        {
            try
            {
                var activity = new Activity
                {
                    PkActivityId = requestDto.ActivityId,
                    Name = requestDto.Name,
                    Description = requestDto.Description,
                    FkActivityTypeId = requestDto.ActivityTypeId
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

        public void UpdateActivityObservation(EditObseravationDto dto)
        {
            try
            {

                var model = new ActivityObservation { PkActivityObservationId = dto.ObseravtionId };

                _context.ActivityObservation.Attach(model);

                model.ClientReview = dto.ClientReview;
                model.ClinetReviewDate = DateTime.Now;
                model.Status = dto.Status;
                model.Description = dto.Description;
                _context.SaveChanges();
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
        /// <param name="requestDto"></param>
        /// <returns></returns>
        /*  public async Task<int> UpdateActivity(StationActivityDto requestDto)
          {
              try
              {
                  var stationActivities = _context.StationActivity
                                          .AsNoTracking()
                                          .Where(i => i.FkStationId ==  requestDto.StationId)
                                          .ToList();
                  _context.StationActivity.RemoveRange(stationActivities);
                  _context.SaveChanges();

                      var activities = requestDto.Activities.Select(i => new Activity
                  {
                      PkActivityId = i.ActivityId,
                      Name = i.Name,
                      Description = i.Description,
                      CreateOn = DateTime.Now,
                      StationActivity = new List<StationActivity>
                      {
                          new StationActivity
                          {
                              FkStationId = requestDto.StationId
                          }
                      }
                  });

                  _context.Activity.UpdateRange(activities);
                  await _context.SaveChangesAsync();
                  return await Task.FromResult(_context.SaveChanges());

              }
              catch (Exception e)
              {
                  Console.WriteLine(e);
                  throw;
              }
          }
          */

       public EditObseravationDto GetActivityObservationById(int Observationid)
        {
            try
            {
                return _context.ActivityObservation
                    .AsNoTracking()
                    .Where(i => i.PkActivityObservationId == Observationid)
                    .Select(p => new EditObseravationDto
                    {
                        ObseravtionId = p.PkActivityObservationId,
                        Description = p.Description,
                        ClientReview = p.ClientReview,
                        Status = p.Status

                    }).Single();
                    
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public int DeleteObservationById(int id)
        {
            try
            {

                var observationid = _context.ActivityObservation.Find(id);
                _context.ActivityObservation.Remove(observationid);
                _context.SaveChanges();
                return observationid.PkActivityObservationId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
