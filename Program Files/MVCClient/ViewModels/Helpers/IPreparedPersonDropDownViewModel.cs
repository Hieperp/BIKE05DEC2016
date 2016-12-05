using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCClient.ViewModels.Helpers
{
    public interface IPreparedPersonDropDownViewModel
    {
        [Display(Name = "Người lập")]
        int PreparedPersonID { get; set; }
        IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
    }
}