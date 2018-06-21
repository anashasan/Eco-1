using System.Collections.Generic;

namespace Host.DataModel
{
    public class StationActivityDto
    {
        public int StationId { get; set; }
        public List<ActivityDto> Activities { get; set; }
    }
}
