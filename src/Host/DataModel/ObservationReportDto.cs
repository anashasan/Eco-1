using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ObservationReportDto
    {
        public int BranchId { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public int ObervationNumber { get; set; }
        public string ObservationDate { get; set; }
        public int ActivityObservationId { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public int StationNumber { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }
        public string ClientReview { get; set; }
        public DateTime? ClientReviewDate { get; set; }
        public string Status { get; set; }
        public string Images { get; set; }

    }
}
