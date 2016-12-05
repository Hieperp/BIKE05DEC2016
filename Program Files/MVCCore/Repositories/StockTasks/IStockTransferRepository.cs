using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.StockTasks
{
    public interface IStockTransferRepository : IGenericWithDetailRepository<StockTransfer, StockTransferDetail>
    {
    }

    public interface IVehicleTransferRepository : IStockTransferRepository
    {
        IEnumerable<PendingVehicleTransferOrder> GetPendingVehicleTransferOrders(int locationID, int transferOrderID);
    }

    public interface IPartTransferRepository : IStockTransferRepository
    {
        IEnumerable<PendingPartTransferOrder> GetPendingPartTransferOrders(int locationID, int transferOrderID);
    }




    public interface IVehicleTransferAPIRepository : IGenericAPIRepository
    {
    }

    public interface IPartTransferAPIRepository : IGenericAPIRepository
    {
    }

}
