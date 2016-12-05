using MVCModel.Models;

namespace MVCCore.Repositories.PurchaseTasks
{
    public interface IPurchaseOrderRepository : IGenericWithDetailRepository<PurchaseOrder, PurchaseOrderDetail>
    {
    }

    public interface IPurchaseOrderAPIRepository : IGenericAPIRepository
    {
    }
}
