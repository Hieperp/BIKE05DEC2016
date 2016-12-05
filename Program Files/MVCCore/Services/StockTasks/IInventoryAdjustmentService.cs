using MVCModel.Models;
using MVCDTO.StockTasks;
using MVCCore.Services.Helpers;

namespace MVCCore.Services.StockTasks
{
    public interface IVehicleAdjustmentService : IGenericWithViewDetailService<InventoryAdjustment, InventoryAdjustmentDetail, VehicleAdjustmentViewDetail, VehicleAdjustmentDTO, VehicleAdjustmentPrimitiveDTO, VehicleAdjustmentDetailDTO>
    {
    }

    public interface IPartAdjustmentService : IGenericWithViewDetailService<InventoryAdjustment, InventoryAdjustmentDetail, PartAdjustmentViewDetail, PartAdjustmentDTO, PartAdjustmentPrimitiveDTO, PartAdjustmentDetailDTO>
    {
    }

    



    public interface IPartAdjustmentHelperService : IHelperService<InventoryAdjustment, InventoryAdjustmentDetail, PartAdjustmentDTO, PartAdjustmentDetailDTO>
    {
    }

}

