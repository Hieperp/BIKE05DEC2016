using System.Web.Mvc;
using System.Collections.Generic;

using MVCDTO.StockTasks;
using MVCClient.ViewModels.Helpers;

namespace MVCClient.ViewModels.StockTasks
{
    public class WarehouseInvoiceViewModel : WarehouseInvoiceDTO, IViewDetailViewModel<WarehouseInvoiceDetailDTO>, IWarehouseAutoCompleteViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel
    {
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    }
}