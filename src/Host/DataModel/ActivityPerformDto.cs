﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ActivityPerformDto
    {
        public int StationLocationId { get; set; }
        public string EmployeeId { get; set; }
        public string StationName { get; set; }
        public string ActivityDateTime { get; set; }
        public List<ActivityPerformDetailDto> Activities { get; set; }

    }
}
