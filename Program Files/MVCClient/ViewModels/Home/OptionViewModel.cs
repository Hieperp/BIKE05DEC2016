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

    public class UpdateBalanceViewModel
    {
        [Display(Name = "Tính tồn kho, bình quân giá vốn kể từ ngày:")]
        public DateTime FromDate { get; set; }

        [Display(Name = "Đánh dấu vào đây để xác nhận")]
        public bool Confirmed { get; set; }
    }
}