using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCClient.ViewModels.Helpers
{
    public interface IOrderInvoiceViewModel : ISupplierAutoCompleteViewModel
    {
        Nullable<int> PurchaseOrderID { get; set; }
        string PurchaseOrderReference { get; set; }
        Nullable<System.DateTime> PurchaseOrderEntryDate { get; set; }
        string PurchaseOrderAttentionName { get; set; }
        string PurchaseOrderDescription { get; set; }
        string PurchaseOrderRemarks { get; set; }

    }
}