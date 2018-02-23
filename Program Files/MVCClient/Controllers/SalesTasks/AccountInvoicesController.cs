using System.Web.Mvc;
using MVCModel.Models;

using MVCCore.Services.SalesTasks;

using MVCDTO.SalesTasks;

using MVCClient.ViewModels.SalesTasks;
using MVCClient.Builders.SalesTasks;
using MVCClient.ViewModels.Helpers;


namespace MVCClient.Controllers.SalesTasks
{
    public class AccountInvoicesController : GenericViewDetailController<AccountInvoice, AccountInvoiceDetail, AccountInvoiceViewDetail, AccountInvoiceDTO, AccountInvoicePrimitiveDTO, AccountInvoiceDetailDTO, AccountInvoiceViewModel>
    {
        public AccountInvoicesController(IAccountInvoiceService accountInvoiceService, IAccountInvoiceViewModelSelectListBuilder accountInvoiceViewModelSelectListBuilder)
            : base(accountInvoiceService, accountInvoiceViewModelSelectListBuilder)
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

        public virtual ActionResult GetPendingSalesInvoices()
        {
            return View();
        }
    }  
}