using System.Collections.Generic;

using MVCModel.Models;

using MVCCore.Services.PurchaseTasks;

using MVCDTO.PurchaseTasks;

using MVCClient.ViewModels.PurchaseTasks;
using MVCClient.Builders.PurchaseTasks;



namespace MVCClient.Controllers.PurchaseTasks
{
    public class PurchaseInvoicesController : GenericViewDetailController<PurchaseInvoice, PurchaseInvoiceDetail, PurchaseInvoiceViewDetail, PurchaseInvoiceDTO, PurchaseInvoicePrimitiveDTO, PurchaseInvoiceDetailDTO, PurchaseInvoiceViewModel>
    {
        private readonly IPurchaseInvoiceService purchaseInvoiceService;

        public PurchaseInvoicesController(IPurchaseInvoiceService purchaseInvoiceService, IPurchaseInvoiceViewModelSelectListBuilder purchaseInvoiceViewModelSelectListBuilder)
            : base(purchaseInvoiceService, purchaseInvoiceViewModelSelectListBuilder, true)
        {
            this.purchaseInvoiceService = purchaseInvoiceService;
        }

        
        protected override ICollection<PurchaseInvoiceViewDetail> GetEntityViewDetails(PurchaseInvoiceViewModel purchaseInvoiceViewModel)
        {
            ICollection<PurchaseInvoiceViewDetail> purchaseInvoiceViewDetails = this.purchaseInvoiceService.GetPurchaseInvoiceViewDetails(purchaseInvoiceViewModel.PurchaseInvoiceID, purchaseInvoiceViewModel.PurchaseOrderID == null ? 0 : (int)purchaseInvoiceViewModel.PurchaseOrderID, purchaseInvoiceViewModel.SupplierID, false);

            return purchaseInvoiceViewDetails;
        }


    }
}
