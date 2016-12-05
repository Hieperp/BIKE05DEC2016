using MVCModel.Models;
using MVCDTO.PurchaseTasks;

namespace MVCCore.Services.PurchaseTasks
{
    public interface IPurchaseOrderService : IGenericWithDetailService<PurchaseOrder, PurchaseOrderDetail, PurchaseOrderDTO, PurchaseOrderPrimitiveDTO, PurchaseOrderDetailDTO>
    {
    }
}
