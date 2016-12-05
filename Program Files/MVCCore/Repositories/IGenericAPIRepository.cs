using System;
using System.Collections.Generic;

using MVCBase.Enums;

namespace MVCCore.Repositories
{
    public interface IGenericAPIRepository : IBaseRepository
    {
        ICollection<TEntityIndex> GetEntityIndexes<TEntityIndex>(string aspUserID, DateTime fromDate, DateTime toDate);
    }
}
