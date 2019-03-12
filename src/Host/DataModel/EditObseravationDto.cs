using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class EditObseravationDto
    {
        public int ObseravtionId { get; set; }
        public string Description { get; set; }
        public string ClientReview { get; set; }
        public string Status { get; set; }
        public int BranchId { get; set; }
        public DateTime? ClientReviewDate { get; set; }

    }
}
