using Host.DataModel;
using Host.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface IStationService
    {
        Task<int> AddStation(StationDto requestDto);
        Task<int> UpdateStation(StationDto requestDto);
        StationDto GetStationById(int id);
        PaginatedList<StationDto> GetAllStationPagination(PagingParams pagingParams);
        List<StationDto> GetAllStation();
        void DeleteStation(int id);
    }
}
