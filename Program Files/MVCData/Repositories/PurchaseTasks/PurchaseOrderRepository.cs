using MVCModel.Models;
using MVCCore.Repositories.PurchaseTasks;

namespace MVCData.Repositories.PurchaseTasks
{
    public class PurchaseOrderRepository : GenericWithDetailRepository<PurchaseOrder, PurchaseOrderDetail>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "PurchaseOrderEditable")
        {
        }

    }

    public class PurchaseOrderAPIRepository : GenericAPIRepository, IPurchaseOrderAPIRepository
    {
        public PurchaseOrderAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetPurchaseOrderIndexes")
        {
        }
    }
}
