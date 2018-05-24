using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.DbServices
{
    public class LocationService : ILocationService
    {
        private readonly EcoDbContext _context;

        public LocationService(EcoDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddLocation(LocationDto requestDto)
        {
            try
            {
                var location = new Location
                {
                    Name = requestDto.Name,
                };
                _context.Location.Add(location);
                _context.SaveChanges();

                var branchLocation = new BranchLocation
                {
                    FkBranchId = requestDto.BranchId,
                    FkLocationId = location.PkLocationId
                };

                _context.BranchLocation.Add(branchLocation);

                return await Task.FromResult(_context.SaveChanges());

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<LocationDto> GetAllLocation()
        {
            try
            {
                return _context.Location
                    .AsNoTracking()
                    .Select(a => new LocationDto
                    {

                        Name = a.Name
                    }).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public List<LocationDto> GetLocationByBranchId(int id)
        {
            try
            {

                var branchLocation = _context.BranchLocation
                    .AsNoTracking()
                    .Where(i => i.FkBranchId == id)
                    .Select(i => new LocationDto
                    {
                        LocationId = i.FkLocation.PkLocationId,
                        Name = i.FkLocation.Name,
                    }).ToList();
                return branchLocation;



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;

            }
        }

        public async Task<int> UpdateLocation(LocationDto requestDto)
        {
            try
            {

                var location = new Location
                {
                    Name = requestDto.Name,

                };
                _context.Location.Update(location);
                _context.SaveChanges();
                return await Task.FromResult(location.PkLocationId);

            }
            catch(Exception e) {

                Console.WriteLine(e);
                throw;
                
                    }
        }

        public SelectList GetLocationByStationId(int id)
        {
            throw new NotImplementedException();
        }

        public LocationDto GetLocationById(int id)
        {
            try
            {
                return _context.Location
                       .AsNoTracking()
                       .Where(i => i.PkLocationId == id)
                       .Select(p => new LocationDto
                       {
                           LocationId = p.PkLocationId,
                           Name = p.Name
                       }).SingleOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
