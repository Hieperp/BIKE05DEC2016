using Kendo.Mvc.UI;
using System;


using System.Web.Mvc;
using System.Collections.Generic;

using MVCBase.Enums;
using MVCDTO.SalesTasks;
using MVCClient.ViewModels.Helpers;


namespace MVCClient.ViewModels.SalesTasks
{
    public class VehiclesInvoiceViewModel : VehiclesInvoiceDTO, IViewDetailViewModel<VehiclesInvoiceDetailDTO>, ICustomerAutoCompleteViewModel, IEmployeeAutoCompleteViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IPaymentTermDropDownViewModel, IPromotionAutoCompleteViewModel
    {
        public IEnumerable<SelectListItem> PaymentTermDropDown { get; set; }
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }

        public override bool PrintAfterClosedSubmit { get { return true; } }
    }

    public class VehiclesInvoiceSchedularViewModel : VehiclesInvoiceDTO, ISchedulerEvent
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public bool IsAllDay { get; set; }
        public string RecurrenceException { get; set; }
        public string RecurrenceRule { get; set; }
        public string Title { get; set; }
    }

    public class PartsInvoiceViewModel : PartsInvoiceDTO, IViewDetailViewModel<PartsInvoiceDetailDTO>, IContractibleInvoiceViewModel, ICustomerAutoCompleteViewModel, IEmployeeAutoCompleteViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IPaymentTermDropDownViewModel, IPromotionAutoCompleteViewModel
    {
        public IEnumerable<SelectListItem> PaymentTermDropDown { get; set; }
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    }

    public class ServicesInvoiceViewModel : ServicesInvoiceDTO, ISimpleViewModel, IContractibleInvoiceViewModel, IReceptionistAutoCompleteViewModel, IEmployeeAutoCompleteViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IPaymentTermDropDownViewModel, IPromotionAutoCompleteViewModel
    {
        public IEnumerable<SelectListItem> PaymentTermDropDown { get; set; }
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }

        public override bool PrintAfterClosedSubmit { get { return true; } }
    }

}