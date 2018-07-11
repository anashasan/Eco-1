using Host.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface IGraphService
    {
        Task<GraphDto> Graph();
        Task<List<ListOfTotalActivityDto>> GetTotalCountActivity();
    }
}
