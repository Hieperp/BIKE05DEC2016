using System.Web.Mvc;
using System.Collections.Generic;

using MVCClient.ViewModels.Helpers;
using MVCDTO.StockTasks;

namespace MVCClient.ViewModels.StockTasks
{
    public class VehicleTransferOrderViewModel : VehicleTransferOrderDTO, IViewDetailViewModel<VehicleTransferOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IWarehouseAutoCompleteViewModel, ILocationAutoCompleteViewModel
    {
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    }

    public class PartTransferOrderViewModel : PartTransferOrderDTO, IViewDetailViewModel<PartTransferOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IWarehouseAutoCompleteViewModel, ILocationAutoCompleteViewModel
    {
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    }
    
}