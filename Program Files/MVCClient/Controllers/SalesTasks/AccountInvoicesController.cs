using System.Web.Mvc;
using MVCModel.Models;

using MVCCore.Services.SalesTasks;

using MVCDTO.SalesTasks;

using MVCClient.ViewModels.SalesTasks;
using MVCClient.Builders.SalesTasks;


namespace MVCClient.Controllers.SalesTasks
{
    public class AccountInvoicesController : GenericViewDetailController<AccountInvoice, AccountInvoiceDetail, AccountInvoiceViewDetail, AccountInvoiceDTO, AccountInvoicePrimitiveDTO, AccountInvoiceDetailDTO, AccountInvoiceViewModel>
    {
        public AccountInvoicesController(IAccountInvoiceService accountInvoiceService, IAccountInvoiceViewModelSelectListBuilder accountInvoiceViewModelSelectListBuilder)
            : base(accountInvoiceService, accountInvoiceViewModelSelectListBuilder)
        {
        }

        public ActionResult PrintDetail(int? id)
        {
            return View(InitPrintViewModel(id));
        }

        public virtual ActionResult GetPendingSalesInvoices()
        {
            return View();
        }
    }  
}