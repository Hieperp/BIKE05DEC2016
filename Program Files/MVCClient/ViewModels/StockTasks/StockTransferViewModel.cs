using System.Web.Mvc;
using System.Collections.Generic;

using MVCClient.ViewModels.Helpers;
using MVCDTO.StockTasks;

namespace MVCClient.ViewModels.StockTasks
{
    public class VehicleTransferViewModel : VehicleTransferDTO, IViewDetailViewModel<VehicleTransferDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IWarehouseAutoCompleteViewModel
    {                
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    }

    public class PartTransferViewModel : PartTransferDTO, IViewDetailViewModel<PartTransferDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IWarehouseAutoCompleteViewModel
    {                
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    } 

}