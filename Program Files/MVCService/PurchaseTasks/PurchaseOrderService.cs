using MVCModel.Models;
using MVCDTO.PurchaseTasks;
using MVCCore.Repositories.PurchaseTasks;
using MVCCore.Services.PurchaseTasks;


namespace MVCService.PurchaseTasks
{
    public class PurchaseOrderService : GenericWithDetailService<PurchaseOrder, PurchaseOrderDetail, PurchaseOrderDTO, PurchaseOrderPrimitiveDTO, PurchaseOrderDetailDTO>, IPurchaseOrderService
    {
        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository)
            : base(purchaseOrderRepository)
        {
        }

    }
}
