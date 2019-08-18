using System;
using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCBase.Enums;

namespace MVCCore.Repositories.StockTasks
{
    public interface IWarehouseInvoiceRepository : IGenericWithDetailRepository<WarehouseInvoice, WarehouseInvoiceDetail>
    {
    }

    public interface IWarehouseInvoiceAPIRepository : IGenericAPIRepository
    {
        IEnumerable<PendingGoodsReceipt> GetPendingGoodsReceipts(string aspUserID, int locationID);
        IEnumerable<PendingGoodsReceiptDetail> GetPendingGoodsReceiptDetails(int goodsReceiptID, string aspUserID, int locationID, int warehouseID, DateTime fromDate, DateTime toDate, int warehouseInvoiceID, string goodsReceiptDetailIDs);

        IEnumerable<PendingStockTransfer> GetPendingStockTransfers(string aspUserID, int locationID);
        IEnumerable<PendingStockTransferDetail> GetPendingStockTransferDetails(int stockTransferID, string aspUserID, int locationID, int sourceWarehouseID, int warehouseID, int stockTransferTypeID, DateTime fromDate, DateTime toDate, int warehouseInvoiceID, string stockTransferDetailIDs);
    }
}
