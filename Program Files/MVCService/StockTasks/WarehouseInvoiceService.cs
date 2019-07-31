using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCModel.Models;
using MVCDTO.StockTasks;
using MVCCore.Repositories.StockTasks;
using MVCCore.Services.StockTasks;

namespace MVCService.StockTasks
{
    public class WarehouseInvoiceService: GenericWithViewDetailService<WarehouseInvoice, WarehouseInvoiceDetail, WarehouseInvoiceViewDetail, WarehouseInvoiceDTO, WarehouseInvoicePrimitiveDTO, WarehouseInvoiceDetailDTO>, IWarehouseInvoiceService
    {
        public WarehouseInvoiceService(IWarehouseInvoiceRepository warehouseInvoiceRepository)
            : base(warehouseInvoiceRepository, "WarehouseInvoicePostSaveValidate", "WarehouseInvoiceSaveRelative", null, "GetWarehouseInvoiceViewDetails")
        {
        }

        public override ICollection<WarehouseInvoiceViewDetail> GetViewDetails(int warehouseInvoiceID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("WarehouseInvoiceID", warehouseInvoiceID) };
            return this.GetViewDetails(parameters);
        }

    }
}
