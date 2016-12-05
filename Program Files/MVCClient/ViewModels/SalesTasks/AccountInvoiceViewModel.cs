using System.Web.Mvc;
using System.Collections.Generic;

using MVCDTO.SalesTasks;
using MVCClient.ViewModels.Helpers;

namespace MVCClient.ViewModels.SalesTasks
{
    public class AccountInvoiceViewModel : AccountInvoiceDTO, IViewDetailViewModel<AccountInvoiceDetailDTO>, ICustomerAutoCompleteViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel
    {
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    }

}