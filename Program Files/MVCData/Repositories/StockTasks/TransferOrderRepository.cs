using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using MVCBase.Enums;
using MVCModel.Models;
using MVCCore.Repositories.StockTasks;


namespace MVCData.Repositories.StockTasks
{
    public abstract class TransferOrderRepository : GenericWithDetailRepository<TransferOrder, TransferOrderDetail>, ITransferOrderRepository
    {
        public TransferOrderRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "TransferOrderEditable", "TransferOrderApproved")
        {
        }
    }

    public class VehicleTransferOrderRepository : TransferOrderRepository, IVehicleTransferOrderRepository
    {
        public VehicleTransferOrderRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities)
        {
        }

        public IList<TransferOrder> SearchTransferOrders(int locationID, string commodityTypeIDList, string searchText)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;

            var queryable = this.TotalBikePortalsEntities.TransferOrders.Where(w => w.Approved && !w.InActive && w.SourceLocationID == locationID && (searchText == null || searchText == "" || w.Reference.Contains(searchText) || w.Warehouse.Code.Contains(searchText) || w.Warehouse.Name.Contains(searchText))).Include(w => w.Warehouse).Include(l => l.Warehouse.Location);
            if (commodityTypeIDList != null)
            {
                List<int> listCommodityTypeID = commodityTypeIDList.Split(',').Select(n => int.Parse(n)).ToList();
                queryable = queryable.Where(t => this.TotalBikePortalsEntities.TransferOrderDetails.Where(td => Math.Round(td.Quantity - td.QuantityTransfer, GlobalEnums.rndQuantity) > 0 && listCommodityTypeID.Contains(td.Commodity.CommodityTypeID)).Select(rd => rd.TransferOrderID).Contains(t.TransferOrderID));
            }

            List<TransferOrder> transferOrders = queryable.ToList();

            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return transferOrders;
        }
    }

    public class PartTransferOrderRepository : TransferOrderRepository, IPartTransferOrderRepository
    {
        public PartTransferOrderRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities)
        {
        }
    }








    public class VehicleTransferOrderAPIRepository : GenericAPIRepository, IVehicleTransferOrderAPIRepository
    {
        public VehicleTransferOrderAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetVehicleTransferOrderIndexes")
        {
        }
    }

    public class PartTransferOrderAPIRepository : GenericAPIRepository, IPartTransferOrderAPIRepository
    {
        public PartTransferOrderAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetPartTransferOrderIndexes")
        {
        }
    }

}
