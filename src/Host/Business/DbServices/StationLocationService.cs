using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                var stationLocation = new StationLocation
                {
                    FkStationId = requestDto.StationId,
                    FkLocationId = requestDto.LocationId,
                    
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
                                          StationLocationId = p.PkStationLocationId
                                      }).ToList();
                return stationLocation;
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
                var stationlocation = new StationLocation
                {
                    PkStationLocationId = requestDto.StationLocationId,
                    FkStationId = requestDto.StationId,
                    FkLocationId = requestDto.LocationId
                };

                _context.StationLocation.Update(stationlocation);
                _context.SaveChanges();

                return await Task.FromResult(stationlocation.PkStationLocationId);

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
                    .Where(i => i.FkLocationId == id)
                    .Select(a => new StationLocationDto
                    {
                        StationLocationId = a.PkStationLocationId,
                        StationId=a.FkStation.PkStationId,
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
