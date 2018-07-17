using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class DownloadPdfDto
    {
        public int StationLocationId { get; set; }
        public int LocationId { get; set; }
        public string StationName { get; set; }
        public string LocationName { get; set; }
        public string Code { get; set; }
        public int SNo { get; set; }
    }
}
