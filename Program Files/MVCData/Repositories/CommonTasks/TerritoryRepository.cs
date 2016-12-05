using System.Linq;
using System.Collections.Generic;


using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;


namespace MVCData.Repositories.CommonTasks
{

    public class TerritoryRepository : ITerritoryRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public TerritoryRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<Territory> GetAllTerritories()
        {
            return this.totalBikePortalsEntities.Territories.ToList();
        }

        public dynamic TerritoriesForTreeView(int? id)
        {
            var commodityCategories = from e in totalBikePortalsEntities.Territories
                            where (id.HasValue ? e.AncestorID == id : e.AncestorID == null)
                            select new
                            {
                                id = e.TerritoryID,
                                Name = e.Name,
                                hasChildren = e.Territories1.Any()
                            };

            return commodityCategories;
        }
    }
}
