using Host.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface IActivityService
    {

        // Task<int> AddActivity(StationActivityDto requestDto);
        // Task<int> UpdateActivity(StationActivityDto requestDto);
        Task<int> AddActivity(ActivityDto requestDto);
        Task<int> UpdateActivity(ActivityDto requestDto);
        ActivityDto GetActivityById(int id);
        List<ActivityDto> GetAllActivity();
       // GetStationActivityDto GetActivityByStationId(int id);
        List<ActivityDto> GetActivityByStationId(int id);
        int DeleteActivityById(int id);
        List<ObservationReportDto> GetObservationReport(int branchId, int? locationId,DateTime? fromDate, DateTime? toDate );
    }
}
