using System.Collections.Generic;
using MVCModel.Models;

namespace MVCCore.Repositories.StockTasks
{
    public interface ITransferOrderRepository : IGenericWithDetailRepository<TransferOrder, TransferOrderDetail>
    {}

    public interface IVehicleTransferOrderRepository : ITransferOrderRepository
    { 
        IList<TransferOrder> SearchTransferOrders(int locationID, string commodityTypeIDList, string searchText);
    }

    public interface IPartTransferOrderRepository : ITransferOrderRepository
    { }





    public interface IVehicleTransferOrderAPIRepository : IGenericAPIRepository
    {
    }

    public interface IPartTransferOrderAPIRepository : IGenericAPIRepository
    {
    }
}
