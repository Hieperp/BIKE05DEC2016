using System.Web.Mvc;
using System.Collections.Generic;

using MVCDTO.CommonTasks;
using MVCClient.ViewModels.Helpers;

namespace MVCClient.ViewModels.CommonTasks
{

    public class CustomerViewModel : CustomerDTO, ISimpleViewModel, IEntireTerritoryAutoCompleteViewModel, ICustomerCategoryDropDownViewModel, ICustomerTypeDropDownViewModel
    {
        public IEnumerable<SelectListItem> CustomerCategoryDropDown { get; set; }
        public IEnumerable<SelectListItem> CustomerTypeDropDown { get; set; }
    }
}