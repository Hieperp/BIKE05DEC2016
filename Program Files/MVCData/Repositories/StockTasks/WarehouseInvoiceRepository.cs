using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using MVCBase.Enums;
using MVCModel.Models;
using MVCCore.Repositories.StockTasks;

namespace MVCData.Repositories.StockTasks
{
    public class WarehouseInvoiceRepository : GenericWithDetailRepository<WarehouseInvoice, WarehouseInvoiceDetail>, IWarehouseInvoiceRepository
    {
        public WarehouseInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities) { }
    }


    public class WarehouseInvoiceAPIRepository : GenericAPIRepository, IWarehouseInvoiceAPIRepository
    {
        public WarehouseInvoiceAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetWarehouseInvoiceIndexes")
        {
        }
        public IEnumerable<PendingStockTransferDetail> GetPendingStockTransferDetails(int stockTransferID, string aspUserID, int locationID, int stockTransferTypeID, DateTime fromDate, DateTime toDate, int warehouseInvoiceID, string stockTransferDetailIDs)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingStockTransferDetail> pendingStockTransfers = base.TotalBikePortalsEntities.GetPendingStockTransferDetails(stockTransferID, aspUserID, locationID, stockTransferTypeID, fromDate, toDate, warehouseInvoiceID, stockTransferDetailIDs).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return pendingStockTransfers;
        }
    }

}
