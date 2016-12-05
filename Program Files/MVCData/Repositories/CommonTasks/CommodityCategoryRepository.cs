using System.Linq;
using System.Collections.Generic;


using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;


namespace MVCData.Repositories.CommonTasks
{

    public class CommodityCategoryRepository : ICommodityCategoryRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public CommodityCategoryRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<CommodityCategory> GetAllCommodityCategories()
        {
            return this.totalBikePortalsEntities.CommodityCategories.ToList();
        }

        public dynamic CommodityCategoriesForTreeView(int? id)
        {
            var commodityCategories = from e in totalBikePortalsEntities.CommodityCategories
                            where (id.HasValue ? e.AncestorID == id : e.AncestorID == null)
                            select new
                            {
                                id = e.CommodityCategoryID,
                                Name = e.Name,
                                hasChildren = e.CommodityCategories1.Any()
                            };

            return commodityCategories;
        }
    }
}
