using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Host.DataModel
{
    public class LocationDto
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public SelectList Station {get; set;}
        public int BranchId { get; set; }
    }
}
