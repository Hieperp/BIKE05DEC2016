using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;

namespace MVCData.Repositories.CommonTasks
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public WarehouseRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<Warehouse> GetAllWarehouses()
        {
            return this.totalBikePortalsEntities.Warehouses.ToList();
        }

        public IList<Warehouse> SearchWarehousesByName(int? locationID, string searchText)
        {
            this.totalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;

            List<Warehouse> warehouses = this.totalBikePortalsEntities.Warehouses.Include("Location").Where(w => ((int)locationID == -1976 || w.LocationID == (int)locationID) && (w.Code.Contains(searchText) || w.Name.Contains(searchText))).ToList();
            
            this.totalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return warehouses;
        }
    }
}
