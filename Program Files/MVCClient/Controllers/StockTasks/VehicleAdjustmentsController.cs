using MVCModel.Models;

using MVCCore.Services.StockTasks;

using MVCDTO.StockTasks;

using MVCClient.ViewModels.StockTasks;
using MVCClient.Builders.StockTasks;


namespace MVCClient.Controllers.StockTasks
{
    public class VehicleAdjustmentsController : GenericViewDetailController<InventoryAdjustment, InventoryAdjustmentDetail, VehicleAdjustmentViewDetail, VehicleAdjustmentDTO, VehicleAdjustmentPrimitiveDTO, VehicleAdjustmentDetailDTO, VehicleAdjustmentViewModel>
    {
        public VehicleAdjustmentsController(IVehicleAdjustmentService vehicleAdjustmentService, IVehicleAdjustmentViewModelSelectListBuilder vehicleAdjustmentViewModelSelectListBuilder)
            : base(vehicleAdjustmentService, vehicleAdjustmentViewModelSelectListBuilder)
        {
        }
    }  
}