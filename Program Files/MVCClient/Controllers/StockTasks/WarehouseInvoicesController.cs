using System.Net;
using System.Web.Mvc;

using MVCBase.Enums;
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

            WarehouseInvoice entity = this.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Readable);
            if (entity == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            printViewModel.ViewOptionID = (entity.Warehouse.LocationID == 9 || entity.Warehouse1.LocationID == 9) ? 9 : entity.Warehouse.LocationID;

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