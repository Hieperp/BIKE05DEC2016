using System.Web.Mvc;
using System.Collections.Generic;

using MVCDTO.SalesTasks;
using MVCClient.ViewModels.Helpers;

namespace MVCClient.ViewModels.SalesTasks
{
    public class ServiceContractViewModel : ServiceContractDTO, ISimpleViewModel, ICustomerAutoCompleteViewModel, IServiceContractTypeDropDownViewModel, ICommodityAutoCompleteViewModel
    {
        public IEnumerable<SelectListItem> ServiceContractTypeDropDown { get; set; }
        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    }
}