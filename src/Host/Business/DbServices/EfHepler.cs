using Host.Business.IDbServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Business.DbServices
{
    public class EfHepler : IEfHepler
    {
        private readonly IAuditDbContext _auditDbContext;
        public EfHepler(IAuditDbContext auditDbContext)
        {
            _auditDbContext = auditDbContext;
        }

        public async Task<IEnumerable<dynamic>> ExecuteProcedure(string procedureName, SqlParameter[] commandParameters)
        {
            using (var cmd = _auditDbContext.Database.GetDbConnection().CreateCommand())
            {
                AddParamAndOpenConnection(procedureName, commandParameters, cmd);

                var retObject = new List<dynamic>();
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    while (await dataReader.ReadAsync())
                    {
                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            dataRow.Add(
                                dataReader.GetName(iFiled),
                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                            );

                        retObject.Add((ExpandoObject)dataRow);
                    }
                }
                return await Task.FromResult(retObject);
            }
        }

        public async Task<int> ExecuteNonQuery(string procedureName, SqlParameter[] commandParameters)
        {
            try
            {
                using (var cmd = _auditDbContext.Database.GetDbConnection().CreateCommand())
                {
                    AddParamAndOpenConnection(procedureName, commandParameters, cmd);
                    return await Task.FromResult(cmd.ExecuteNonQuery());
                }
            }
            finally
            {
                if (_auditDbContext.Database.GetDbConnection().State == ConnectionState.Open)
                    _auditDbContext.Database.GetDbConnection().Close();
            }
        }

        public async Task<int> ExecuteScalar(string procedureName, SqlParameter[] commandParameters)
        {
            try
            {
                using (var cmd = _auditDbContext.Database.GetDbConnection().CreateCommand())
                {
                    AddParamAndOpenConnection(procedureName, commandParameters, cmd);
                    return await Task.FromResult((int)cmd.ExecuteScalar());
                }
            }
            finally
            {
                if (_auditDbContext.Database.GetDbConnection().State == ConnectionState.Open)
                    _auditDbContext.Database.GetDbConnection().Close();
            }
        }


        private static void AddParamAndOpenConnection(string procedureName, SqlParameter[] commandParameters, DbCommand cmd)
        {
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            if (commandParameters != null)
                cmd.Parameters.AddRange(commandParameters);
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
        }
    }
}
