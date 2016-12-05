using System;
using System.ComponentModel.DataAnnotations;

namespace MVCClient.ViewModels.Helpers
{
    public interface IEntireTerritoryAutoCompleteViewModel
    {
        [Display(Name = "Khu vực")]
        int TerritoryID { get; set; }
        string EntireTerritoryEntireName { get; set; }       
    }
}
