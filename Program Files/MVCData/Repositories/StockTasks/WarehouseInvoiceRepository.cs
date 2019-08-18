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

        public IEnumerable<PendingGoodsReceipt> GetPendingGoodsReceipts(string aspUserID, int locationID)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsReceipt> pendingGoodsReceipts = base.TotalBikePortalsEntities.GetPendingGoodsReceipts(aspUserID, locationID).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsReceipts;
        }

        public IEnumerable<PendingGoodsReceiptDetail> GetPendingGoodsReceiptDetails(int goodsReceiptID, string aspUserID, int locationID, int warehouseID, DateTime fromDate, DateTime toDate, int warehouseInvoiceID, string goodsReceiptDetailIDs)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsReceiptDetail> pendingGoodsReceipts = base.TotalBikePortalsEntities.GetPendingGoodsReceiptDetails(goodsReceiptID, aspUserID, locationID, warehouseID, fromDate, toDate, warehouseInvoiceID, goodsReceiptDetailIDs).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsReceipts;
        }

        public IEnumerable<PendingStockTransfer> GetPendingStockTransfers(string aspUserID, int locationID)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingStockTransfer> pendingStockTransfers = base.TotalBikePortalsEntities.GetPendingStockTransfers(aspUserID, locationID).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return pendingStockTransfers;
        }

        public IEnumerable<PendingStockTransferDetail> GetPendingStockTransferDetails(int stockTransferID, string aspUserID, int locationID, int sourceWarehouseID, int warehouseID, int stockTransferTypeID, DateTime fromDate, DateTime toDate, int warehouseInvoiceID, string stockTransferDetailIDs)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingStockTransferDetail> pendingStockTransfers = base.TotalBikePortalsEntities.GetPendingStockTransferDetails(stockTransferID, aspUserID, locationID, sourceWarehouseID, warehouseID, stockTransferTypeID, fromDate, toDate, warehouseInvoiceID, stockTransferDetailIDs).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return pendingStockTransfers;
        }
    }

}
