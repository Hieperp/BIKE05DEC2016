using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCModel.Models;
using MVCCore.Repositories.PurchaseTasks;
using MVCData.Helpers;

namespace MVCData.Repositories.PurchaseTasks
{
    public class PurchaseInvoiceRepository : GenericWithDetailRepository<PurchaseInvoice, PurchaseInvoiceDetail>, IPurchaseInvoiceRepository
    {
        public PurchaseInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "PurchaseInvoiceEditable")
        {
        }

        public ICollection<PurchaseInvoiceGetPurchaseOrder> GetPurchaseOrders(int locationID, int? purchaseInvoiceID, string purchaseOrderReference)
        {
            return this.TotalBikePortalsEntities.PurchaseInvoiceGetPurchaseOrders(locationID, purchaseInvoiceID, purchaseOrderReference).ToList();
        }

        public ICollection<PurchaseInvoiceGetSupplier> GetSuppliers(int locationID, int? purchaseInvoiceID, string supplierName)
        {
            return this.TotalBikePortalsEntities.PurchaseInvoiceGetSuppliers(locationID, purchaseInvoiceID, supplierName).ToList();
        }
    }


    public class PurchaseInvoiceAPIRepository : GenericAPIRepository, IPurchaseInvoiceAPIRepository
    {
        public PurchaseInvoiceAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetPurchaseInvoiceIndexes")
        {
        }
    }
}
