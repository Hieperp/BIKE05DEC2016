using System.Web.Mvc;
using System.Collections.Generic;

using MVCBase.Enums;
using MVCDTO.PurchaseTasks;
using MVCClient.ViewModels.Helpers;

namespace MVCClient.ViewModels.PurchaseTasks
{
    public class PurchaseInvoiceViewModel : PurchaseInvoiceDTO, IOrderInvoiceViewModel, IViewDetailViewModel<PurchaseInvoiceDetailDTO>, IPriceTermDropDownViewModel, IPaymentTermDropDownViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel
    {
        public IEnumerable<SelectListItem> PriceTermDropDown { get; set; }
        public IEnumerable<SelectListItem> PaymentTermDropDown { get; set; }
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }         
    }
}