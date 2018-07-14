using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class StationLocationDto
    {
        public int StationLocationId { get; set; }
        public SelectList Stations { get; set; }
        public int StationId { get; set; }
        public int LocationId { get; set; }
        public int BranchId { get; set; }
        public string StationName { get; set; }
        public string Code { get; set; }
        public int Sno { get; set; }

    }
}
