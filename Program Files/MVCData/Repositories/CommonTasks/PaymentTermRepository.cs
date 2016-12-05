using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories.CommonTasks
{
    public class PaymentTermRepository : IPaymentTermRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public PaymentTermRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<PaymentTerm> GetAllPaymentTerms()
        {
            return this.totalBikePortalsEntities.PaymentTerms.ToList();
        }
    }
}
