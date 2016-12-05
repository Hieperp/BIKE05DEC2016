using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCCore.Repositories.StockTasks;

namespace MVCData.Repositories.StockTasks
{
    public abstract class StockTransferRepository : GenericWithDetailRepository<StockTransfer, StockTransferDetail>, IStockTransferRepository
    {
        public StockTransferRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "StockTransferEditable")
        {
        }

    }





    public class VehicleTransferRepository : StockTransferRepository, IVehicleTransferRepository
    {
        public VehicleTransferRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities)
        {
        }

        public IEnumerable<PendingVehicleTransferOrder> GetPendingVehicleTransferOrders(int locationID, int transferOrderID)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingVehicleTransferOrder> pendingVehicleTransferOrders = this.TotalBikePortalsEntities.GetPendingVehicleTransferOrders(locationID, transferOrderID).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return pendingVehicleTransferOrders;
        }
    }





    public class PartTransferRepository : StockTransferRepository, IPartTransferRepository
    {
        public PartTransferRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities)
        {
        }

        public IEnumerable<PendingPartTransferOrder> GetPendingPartTransferOrders(int locationID, int transferOrderID)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingPartTransferOrder> pendingPartTransferOrders = this.TotalBikePortalsEntities.GetPendingPartTransferOrders(locationID, transferOrderID).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPartTransferOrders;
        }
    }








    public class VehicleTransferAPIRepository : GenericAPIRepository, IVehicleTransferAPIRepository
    {
        public VehicleTransferAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetVehicleTransferIndexes")
        {
        }
    }

    public class PartTransferAPIRepository : GenericAPIRepository, IPartTransferAPIRepository
    {
        public PartTransferAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetPartTransferIndexes")
        {
        }
    }

}
