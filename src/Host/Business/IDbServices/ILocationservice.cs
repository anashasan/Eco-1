using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.DataModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Host.Business.IDbServices
{
  public interface ILocationService
    {
        Task<int> AddLocation(LocationDto requestDto);
       List <LocationDto> GetLocationByBranchId(int id);
        List<LocationDto> GetAllLocation();
        SelectList GetLocationByStationId(int id);
        Task<int> UpdateLocation(LocationDto requestDto);
        LocationDto GetLocationById(int id);
            
    }
}
