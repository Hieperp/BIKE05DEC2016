using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;

namespace MVCData.Repositories.CommonTasks
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public PromotionRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<Promotion> SearchPromotions(int? locationID, string searchText)
        {
            this.totalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;

            List<Promotion> Promotions = this.totalBikePortalsEntities.Promotions.Where(w => (w.Code.Contains(searchText) || w.Name.Contains(searchText))).ToList(); //((int)locationID == -1976 || w.LocationID == (int)locationID) && 

            this.totalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return Promotions;
        }
    }
}
