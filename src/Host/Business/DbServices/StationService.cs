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
    public class StationService : IStationService
    {
        private readonly EcoDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public StationService(EcoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> AddStation(StationDto requestDto)
        {
            try
            {
                var station = new Station
                {
                    Description = requestDto.Description,
                    Name = requestDto.Name,
                    CreateOn = DateTime.Now,
                };

                _context.Station.Add(station);
                _context.SaveChanges();

                return await Task.FromResult(station.PkStationId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> DeleteStation(int id)
        {
            var deleteStation = _context.Station.Find(id);
            _context.Station.Remove(deleteStation);
            return await Task.FromResult(_context.SaveChanges());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GetStationDto> GetAllStation()
        {
            return _context.Station
                   .AsNoTracking()
                   .Select(i => new GetStationDto
                   {
                       Name = i.Name,
                       StationId = i.PkStationId,
                       Description = i.Description,
                       Activities = i.StationActivity.Select(p => p.FkActivity.Name).ToList()
                   }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StationDto GetStationById(int id)
        {
            return _context.Station
                   .AsNoTracking()
                   .Where(p => p.PkStationId == id)
                   .Select(i => new StationDto
                   {
                       Name = i.Name,
                       StationId = i.PkStationId,
                       Description = i.Description
                   }).Single();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> UpdateStation(StationDto requestDto)
        {
            try
            {
                var station = new Station
                {
                    PkStationId = requestDto.StationId,
                    Description = requestDto.Description,
                    Name = requestDto.Name,
                    UpdatedOn = DateTime.Now,
                };

                _context.Station.Update(station);
                _context.SaveChanges();

                return await Task.FromResult(station.PkStationId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
