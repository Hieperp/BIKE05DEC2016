using System.Collections.Generic;

using MVCModel.Models;


namespace MVCCore.Repositories.PurchaseTasks
{
    public interface IPurchaseInvoiceRepository : IGenericWithDetailRepository<PurchaseInvoice, PurchaseInvoiceDetail>
    {
        ICollection<PurchaseInvoiceGetPurchaseOrder> GetPurchaseOrders(int locationID, int? purchaseInvoiceID, string purchaseOrderReference);
        ICollection<PurchaseInvoiceGetSupplier> GetSuppliers(int locationID, int? purchaseInvoiceID, string supplierName);
    }

    public interface IPurchaseInvoiceAPIRepository : IGenericAPIRepository
    {
    }
}
