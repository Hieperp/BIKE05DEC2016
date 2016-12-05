using System.Collections.Generic;

using MVCModel.Models;
using MVCDTO.PurchaseTasks;


namespace MVCCore.Services.PurchaseTasks
{
    public interface IPurchaseInvoiceService : IGenericWithViewDetailService<PurchaseInvoice, PurchaseInvoiceDetail, PurchaseInvoiceViewDetail, PurchaseInvoiceDTO, PurchaseInvoicePrimitiveDTO, PurchaseInvoiceDetailDTO>
    {
        ICollection<PurchaseInvoiceViewDetail> GetPurchaseInvoiceViewDetails(int purchaseInvoiceID, int purchaseOrderID, int supplierID, bool isReadOnly);
    }
}
