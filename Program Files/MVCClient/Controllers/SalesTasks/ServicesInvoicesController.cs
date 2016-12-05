using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using MVCBase.Enums;

using MVCModel.Models;

using MVCCore.Services.SalesTasks;

using MVCDTO.SalesTasks;

using MVCClient.ViewModels.SalesTasks;
using MVCClient.Builders.SalesTasks;



namespace MVCClient.Controllers.SalesTasks
{
    public class ServicesInvoicesController : GenericSimpleController<SalesInvoice, ServicesInvoiceDTO, ServicesInvoicePrimitiveDTO, ServicesInvoiceViewModel>
    {
        public ServicesInvoicesController(IServicesInvoiceService servicesInvoiceService, IServicesInvoiceViewModelSelectListBuilder servicesInvoiceViewModelSelectListBuilder)
            : base(servicesInvoiceService, servicesInvoiceViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Services);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditServiceContract(ServicesInvoiceViewModel simpleViewModel)
        {
            return RedirectToAction("Edit", "ServiceContracts", new { id = simpleViewModel.ServiceContractID });
        }


        public virtual ActionResult GetQuotationDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }


        //[AccessLevelAuthorize(GlobalEnums.AccessLevel.Readable)]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult SearchByVehicle(int? id)
        {
            ViewBag.ServiceContractID = id;
            ViewBag.SelectedEntityID = -1;
            return View("Index");
        }

    }
}
