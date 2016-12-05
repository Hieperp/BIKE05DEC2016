using MVCModel.Models;
using MVCDTO.StockTasks;

namespace MVCCore.Services.StockTasks
{
    public interface IVehicleTransferOrderService : IGenericWithViewDetailService<TransferOrder, TransferOrderDetail, VehicleTransferOrderViewDetail, VehicleTransferOrderDTO, VehicleTransferOrderPrimitiveDTO, VehicleTransferOrderDetailDTO>
    {
        bool InitWarehouseBalance15AUG();
    }

    public interface IPartTransferOrderService : IGenericWithViewDetailService<TransferOrder, TransferOrderDetail, PartTransferOrderViewDetail, PartTransferOrderDTO, PartTransferOrderPrimitiveDTO, PartTransferOrderDetailDTO>
    {
    }

}

