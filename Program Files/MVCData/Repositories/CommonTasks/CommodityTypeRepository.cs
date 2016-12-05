using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;

namespace MVCData.Repositories.CommonTasks
{
    public class CommodityTypeRepository : ICommodityTypeRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public CommodityTypeRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<CommodityType> GetAllCommodityTypes()
        {
            return this.totalBikePortalsEntities.CommodityTypes.ToList();
        }

        public dynamic CommodityTypesForTreeView(int? id)
        {
            var commodityTypes = from e in totalBikePortalsEntities.CommodityTypes
                                 where (id.HasValue ? e.AncestorID == id : e.AncestorID == null)
                                 select new
                                 {
                                     id = e.CommodityTypeID,
                                     Name = e.Name,
                                     hasChildren = e.CommodityTypes1.Any()
                                 };

            return commodityTypes;
        }
    }
      
}
