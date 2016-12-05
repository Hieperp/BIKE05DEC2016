using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCBase.Enums;


namespace MVCCore.Repositories.StockTasks
{
    public interface IInventoryAdjustmentRepository : IGenericWithDetailRepository<InventoryAdjustment, InventoryAdjustmentDetail>
    {
    }

    public interface IVehicleAdjustmentRepository : IInventoryAdjustmentRepository
    {
    }

    public interface IPartAdjustmentRepository : IInventoryAdjustmentRepository
    {
    }


    public interface IVehicleAdjustmentAPIRepository : IGenericAPIRepository
    {
    }

    public interface IPartAdjustmentAPIRepository : IGenericAPIRepository
    {
    }
}
