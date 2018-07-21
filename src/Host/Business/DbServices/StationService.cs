using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Host.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;

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

        public void DeleteStation(int id)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                connection.Execute(
                    "[dbo].[usp_DeleteStation]"
                    , new { @paramStationId = id },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<StationDto> GetAllStation()
        {
            try
            {
               return _context.Station
                   .AsNoTracking()
                   .Select(i => new StationDto
                   {
                       Name = i.Name,
                       StationId = i.PkStationId,
                       Description = i.Description,

                   }).ToList();
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
        public PaginatedList<StationDto> GetAllStationPagination(PagingParams pagingParams)
        {
            
            var station = _context.Station
                   .AsNoTracking()
                   .Select(i => new StationDto
                   {
                       Name = i.Name,
                       StationId = i.PkStationId,
                       Description = i.Description,
                       
                   }).ToList();
            var model = new PaginatedList<StationDto>(station.AsQueryable(), pagingParams.PageNumber, pagingParams.PageSize);
            return model;
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
                    PkStationId = requestDto.StationId.Value,
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
