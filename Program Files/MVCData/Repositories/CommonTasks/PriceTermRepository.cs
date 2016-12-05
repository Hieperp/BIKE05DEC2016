using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;

namespace MVCData.Repositories.CommonTasks
{
    public class PriceTermRepository : IPriceTermRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public PriceTermRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<PriceTerm> GetAllPriceTerms()
        {
            return this.totalBikePortalsEntities.PriceTerms.ToList();
        }

    }
}
