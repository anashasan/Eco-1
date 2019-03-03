using Host.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface IActivityPerformService
    {
        Task<int> ActivityPerform(ActivityPerformDto requestDto);
        //Task<int> ActivityPerform1(ActitvityPerformDto1 requestDto1);
        Task<List<GroupActivityReports>> ActivityFilterReport();
        Task<List<GroupActivityReports>> ActivityFilterReporByBranchIdt(int branchId, int locationId);
        // Task<List<ReportDto>> ActivityReport(int? locationId, DateTime? createdOn);
        Task<List<ReportDto>> ActivityReport(int? locationId, DateTime? fromDate, DateTime? toDate, int? branchId);
        List<Graph> StationReport(int? locationId, DateTime? fromDate, DateTime? toDate, int? branchId);
        List<GetDailyReportDto> GetDailyReportByBranchId(int? locationId, DateTime? fromDate, DateTime? toDate, int? branchId);
        void UpdateDailyReport(IEnumerable<GetDailyReportDto> dailyReports);
        //List<StationReportDto> StationReport();
    }
}
