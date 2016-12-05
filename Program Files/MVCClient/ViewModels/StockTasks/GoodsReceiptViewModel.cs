using System.Web.Mvc;
using System.Collections.Generic;

using MVCDTO.StockTasks;
using MVCClient.ViewModels.Helpers;

namespace MVCClient.ViewModels.StockTasks
{
    public class GoodsReceiptViewModel : GoodsReceiptDTO, IViewDetailViewModel<GoodsReceiptDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel
    {
        public string VoucherText1 { get; set; }
        public string VoucherText2 { get; set; }
        public string VoucherText3 { get; set; }
        public string VoucherText4 { get; set; }
        public string VoucherText5 { get; set; }

        public IEnumerable<SelectListItem> PreparedPersonDropDown { get; set; }
        public IEnumerable<SelectListItem> ApproverDropDown { get; set; }
    }
}