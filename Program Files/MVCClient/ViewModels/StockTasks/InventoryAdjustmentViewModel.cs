using System;

using System.Web.Mvc;
using System.Collections.Generic;

using MVCBase.Enums;
using MVCDTO.StockTasks;
using MVCClient.ViewModels.Helpers;

namespace MVCClient.ViewModels.StockTasks
{
    public class VehicleAdjustmentViewModel : VehicleAdjustmentDTO, IViewDetailViewModel<VehicleAdjustmentDetailDTO>, ISupplierAutoCompleteViewModel, IEmployeeAutoCompleteViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IPaymentTermDropDownViewModel
    {
        public IEnumerable<SelectListItem> PaymentTermDropDown { get; set; }
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    }

    public class PartAdjustmentViewModel : PartAdjustmentDTO, IViewDetailViewModel<PartAdjustmentDetailDTO>, ISupplierAutoCompleteViewModel, IEmployeeAutoCompleteViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IPaymentTermDropDownViewModel
    {
        public IEnumerable<SelectListItem> PaymentTermDropDown { get; set; }
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    }

}