using System.Collections.Generic;
using System.Linq;

using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;

namespace MVCData.Repositories.CommonTasks
{   
    public class EntireTerritoryRepository : GenericRepository<EntireTerritory>, IEntireTerritoryRepository
    {
        public EntireTerritoryRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities)
        {
        }   

        public IList<EntireTerritory> SearchEntireTerritoriesByName(string name)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            List<EntireTerritory> customers = this.TotalBikePortalsEntities.EntireTerritories.Where(w => w.EntireName.Contains(name)).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return customers;
        }

        public IList<EntireTerritory> GetAllEntireTerritories()
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            List<EntireTerritory> customers = this.TotalBikePortalsEntities.EntireTerritories.ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return customers;
        }
                
    }
}
