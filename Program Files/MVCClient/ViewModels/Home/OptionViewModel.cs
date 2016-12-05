using System;
using System.ComponentModel.DataAnnotations;

namespace MVCClient.ViewModels.Home
{
    public class OptionViewModel
    {
        [Display(Name = "Lọc dữ liệu từ ngày")]
        public DateTime GlobalFromDate { get; set; }
        [Display(Name = "Lọc dữ liệu đến ngày")]
        public DateTime GlobalToDate { get; set; }
    }
}