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
        IEnumerable<PendingStockTransfer> GetPendingStockTransfers(string aspUserID, int locationID);
        IEnumerable<PendingStockTransferDetail> GetPendingStockTransferDetails(int stockTransferID, string aspUserID, int locationID, int stockTransferTypeID, DateTime fromDate, DateTime toDate, int warehouseInvoiceID, string stockTransferDetailIDs);
    }
}
