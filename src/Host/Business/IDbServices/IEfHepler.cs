using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface IEfHepler
    {
        Task<IEnumerable<dynamic>> ExecuteProcedure(string procedureName, SqlParameter[] commandParameters);
        Task<int> ExecuteNonQuery(string procedureName, SqlParameter[] commandParameters);
        Task<int> ExecuteScalar(string procedureName, SqlParameter[] commandParameters);
    }
}
