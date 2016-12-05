using System.Web.Mvc;
using System.Collections.Generic;

using MVCDTO.CommonTasks;
using MVCClient.ViewModels.Helpers;

namespace MVCClient.ViewModels.CommonTasks
{
    public class CommodityViewModel : CommodityDTO, ISimpleViewModel, ISupplierDropDownViewModel, ICommodityCategoryDropDownViewModel, ICommodityTypeDropDownViewModel
    {
        public IEnumerable<SelectListItem> CommodityCategoryDropDown { get; set; }
        public IEnumerable<SelectListItem> CommodityTypeDropDown { get; set; }
        public IEnumerable<SelectListItem> SupplierDropDown { get; set; }        
    }
}