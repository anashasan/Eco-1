using Host.Business.IDbServices;
using Host.DataContext;
using Host.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.DbServices
{
    public class GraphService : IGraphService
    {
        private readonly EcoDbContext _context;
        private readonly IEfHepler _efHepler;

        public GraphService(EcoDbContext context,
                            IEfHepler efHepler)
        {
            _context = context;
            _efHepler = efHepler;
        }


        public async  Task<GraphDto> Graph()
        {
            try
            {

                var act = _context.Set.FromSql("[dbo].[usp_GetAllActivity] @paramStationId ={2023}");

                var abc = _efHepler.ExecuteProcedure(
                "[dbo].[usp_GetAllActivity]",
                new[]
                {
                        new SqlParameter("@paramStationId",2023)
                });
                //Model = (from IDictionary<string, object> model in await abc
                //         select new GraphDto
                //         {
                //             Activity = model.GetSafe<int>("Activity"),

                //         }).ToList();
                var g = new GraphDto();
                return await Task.FromResult(g);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
