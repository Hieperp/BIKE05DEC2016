using System.Web.Mvc;
using MVCModel.Models;

using MVCCore.Services.StockTasks;

using MVCDTO.StockTasks;

using MVCClient.ViewModels.StockTasks;
using MVCClient.Builders.StockTasks;
using MVCClient.ViewModels.Helpers;


namespace MVCClient.Controllers.StockTasks
{
    public class WarehouseInvoicesController : GenericViewDetailController<WarehouseInvoice, WarehouseInvoiceDetail, WarehouseInvoiceViewDetail, WarehouseInvoiceDTO, WarehouseInvoicePrimitiveDTO, WarehouseInvoiceDetailDTO, WarehouseInvoiceViewModel>
    {
        public WarehouseInvoicesController(IWarehouseInvoiceService warehouseInvoiceService, IWarehouseInvoiceViewModelSelectListBuilder warehouseInvoiceViewModelSelectListBuilder)
            : base(warehouseInvoiceService, warehouseInvoiceViewModelSelectListBuilder, true)
        {
        }

        [OnResultExecutingFilterAttribute]
        public ActionResult PrintInvoice(int? id, int? pgid)
        {
            PrintViewModel printViewModel = InitPrintViewModel(id);
            printViewModel.PrintOptionID = (int)pgid;
            return View(printViewModel);
        }

        protected override ViewModels.Helpers.PrintViewModel InitPrintViewModel(int? id)
        {
            PrintViewModel printViewModel = base.InitPrintViewModel(id);
            printViewModel.ViewName = "PrintInvoice";
            return printViewModel;
        }

        public virtual ActionResult GetPendingStockTransferDetails()
        {
            return View();
        }
    }
}