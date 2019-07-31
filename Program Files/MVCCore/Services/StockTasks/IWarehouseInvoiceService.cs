using System.Collections.Generic;

using MVCModel.Models;
using MVCDTO.StockTasks;

namespace MVCCore.Services.StockTasks
{
    public interface IWarehouseInvoiceService : IGenericWithViewDetailService<WarehouseInvoice, WarehouseInvoiceDetail, WarehouseInvoiceViewDetail, WarehouseInvoiceDTO, WarehouseInvoicePrimitiveDTO, WarehouseInvoiceDetailDTO>
    {
        bool Save(WarehouseInvoiceDTO dto, bool useExistingTransaction);
    }
}
