﻿using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Host.Helper;

namespace Host.Business.DbServices
{
    public class StationLocationService : IStationLocationService
    {

        private readonly EcoDbContext _context;

        public StationLocationService(EcoDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddStationLocation(StationLocationDto requestDto)
        {

            try
            {
                var code = LastCodeStationLocation();
                var decriptCode = EncoderAgent.EncryptString(code + "00001");
                var stationLocation = new StationLocation
                {
                    FkStationId = requestDto.StationId,
                    FkLocationId = requestDto.LocationId,
                    Code = decriptCode

                };

                _context.StationLocation.Add(stationLocation);
                _context.SaveChanges();
                return await Task.FromResult(stationLocation.PkStationLocationId);
            }
            catch (Exception e)
            {
                Console.WriteLine();
                throw;
            }
        }

        public List<StationLocationDto> GetStationLocationByLocationId(int locationId)
        {
            try
            {
                var stationLocation = _context.StationLocation
                                      .AsNoTracking()
                                      .Where(i => i.FkLocationId == locationId)
                                      .Select(p => new StationLocationDto
                                      {
                                          StationId = p.FkStation.PkStationId,
                                          StationName = p.FkStation.Name,
                                          StationLocationId = p.PkStationLocationId,
                                          Code = EncoderAgent.DecryptString(p.Code)
                                      }).ToList();
                return stationLocation;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public List<BranchStationLocationDto> GetStationLocationByBranchId(int branchId)
        {
            try
            {
                var locationId = _context.BranchLocation
                                .AsNoTracking()
                                .Where(i => i.FkBranchId == branchId)
                                .Select(i => i.FkLocationId)
                                .ToList();
                var branch = _context.StationLocation
                            .AsNoTracking()
                            .Where(i => locationId.Contains(i.FkLocationId))
                            .Select(p => new BranchStationLocationDto
                            {
                                StationId = p.FkStation.PkStationId,
                                StationName=p.FkStation.Name,
                                LocationId = p.FkLocationId,
                                LocationName = p.FkLocation.Name
                                

                                
                            }).ToList();

                return branch;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;

            }
        }

        public async Task<int> UpdateStationLocation(StationLocationDto requestDto)
        {
            try
            {
                var stationLocation = _context.StationLocation.Find(requestDto.StationLocationId);
                stationLocation.FkStationId = requestDto.StationId;
                stationLocation.FkLocationId = requestDto.LocationId;
                //var stationlocation = new StationLocation
                //{
                //    PkStationLocationId = requestDto.StationLocationId,
                //    FkStationId = requestDto.StationId,
                //    FkLocationId = requestDto.LocationId
                //};

                _context.StationLocation.Update(stationLocation);
                _context.SaveChanges();

                return await Task.FromResult(stationLocation.PkStationLocationId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public StationLocationDto GetStationLocationById(int id)
        {
            try
            {
                return _context.StationLocation
                    .AsNoTracking()
                    .Where(i => i.PkStationLocationId == id)
                    .Select(a => new StationLocationDto
                    {
                        StationLocationId = a.PkStationLocationId,
                        StationId = a.FkStation.PkStationId,
                    }).SingleOrDefault();


            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }

        }

        public string GetStationNameById(int id)
        {
            return _context.Station
                    .AsNoTracking()
                    .Where(i => i.PkStationId == id)
                    .Select(o => o.Name)
                    .SingleOrDefault();
        }

        public string LastCodeStationLocation()
        {
            try
            {
                var lastCode = _context.StationLocation
                               .AsNoTracking()
                               .Select(i => i.PkStationLocationId)
                               .LastOrDefault();
                return lastCode.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ActivityPerformDto> GetStationActivityByCode(string code)
        {
            var encrptCode = EncoderAgent.EncryptString(code);
            var stationId = _context.StationLocation
                            .AsNoTracking()
                            .Where(i => i.Code == encrptCode)
                            .Select(p => new { p.FkStationId, p.FkStation.Name, p.PkStationLocationId })
                            .SingleOrDefault();
            if(stationId !=null && stationId.FkStationId != 0)
            {
                var activities = _context.StationActivity
                             .AsNoTracking()
                             .Where(i => i.FkStationId == stationId.FkStationId)
                             .Select(p => new ActivityPerformDetailDto
                             {
                                 Name = p.FkActivity.Name,
                                 ActivityId = p.FkActivityId,
                                 ActivityTypeId = p.FkActivity.FkActivityTypeId,
                                 StationActivityId = p.PkStationActivityId,
                                 Type = p.FkActivity.FkActivityType.Type
                             })
                             .ToList();
                return await Task.FromResult(new ActivityPerformDto
                {
                    Activities = activities,
                    StationId = stationId.PkStationLocationId,
                    StationName = stationId.Name
                });
            }
            return await Task.FromResult(new ActivityPerformDto
            {
              
            });


        }

        public void DeleteStationLocation(int id)
        {
            try
            {
                var deleteModel = _context.StationLocation
                                              .Where(i => i.PkStationLocationId == id)
                                              .Single();
                _context.StationLocation.Remove(deleteModel);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }



        /*  public List<StationLocationDto> GetStationByLocationId(int id)

          {
              try
              {
                  var stattionlocation = _context.StationLocation
                      .AsNoTracking()
                      .Where(a => a.FkBranchLocation.FkLocationId == id)
                      .Select(i => new StationLocationDto
                      {
                          StationLocationId = i

                      });

              }
              catch (Exception e)
              {
                  Console.WriteLine();
                  throw;
              }
          }*/
    }
}
