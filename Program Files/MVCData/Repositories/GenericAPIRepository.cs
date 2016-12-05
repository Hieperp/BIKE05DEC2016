using MVCBase.Enums;
using MVCCore.Repositories;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories
{
    public class GenericAPIRepository : BaseRepository, IGenericAPIRepository
    {
        private readonly string functionNameGetEntityIndexes;

        public GenericAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities, string functionNameGetEntityIndexes)
            : base(totalBikePortalsEntities)
        {
            this.functionNameGetEntityIndexes = functionNameGetEntityIndexes;
        }

        public virtual ICollection<TEntityIndex> GetEntityIndexes<TEntityIndex>(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("AspUserID", aspUserID), new ObjectParameter("FromDate", fromDate), new ObjectParameter("ToDate", toDate) };

            return base.ExecuteFunction<TEntityIndex>(this.functionNameGetEntityIndexes, parameters);
        }

    }
}
