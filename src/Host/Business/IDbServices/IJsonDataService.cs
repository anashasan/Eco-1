using Host.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Business.IDbServices
{
    public interface IJsonDataService
    {
        JsonDataDto GetJsonData(Guid code, out string message);
    }
}
