using System.Net;
using System.Web.Mvc;
using System.Text;

using AutoMapper;
using RequireJsNet;

using MVCBase.Enums;

using MVCModel.Models;

using MVCCore.Services.SalesTasks;

using MVCDTO.SalesTasks;

using MVCClient.ViewModels.SalesTasks;
using MVCClient.Builders.SalesTasks;




namespace MVCClient.Controllers.SalesTasks
{
    public class PartsInvoicesController : GenericViewDetailController<SalesInvoice, SalesInvoiceDetail, PartsInvoiceViewDetail, PartsInvoiceDTO, PartsInvoicePrimitiveDTO, PartsInvoiceDetailDTO, PartsInvoiceViewModel>
    {
        public PartsInvoicesController(IPartsInvoiceService partsInvoiceService, IPartsInvoiceViewModelSelectListBuilder partsInvoiceViewModelSelectListBuilder)
            : base(partsInvoiceService, partsInvoiceViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Parts);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Consumables);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }

        
        public virtual ActionResult GetQuotationDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }


        [AccessLevelAuthorize]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Promotion(int? id)
        {
            SalesInvoice salesInvoice;
            if (id == null || id <= 0 || (salesInvoice = this.GenericService.GetByID((int)id)) == null || salesInvoice.SalesInvoiceTypeID != (int)GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            PartsInvoiceViewModel partsInvoiceViewModel = new PartsInvoiceViewModel();

            partsInvoiceViewModel.EntryDate = salesInvoice.EntryDate;

            partsInvoiceViewModel.ServiceInvoiceID = salesInvoice.SalesInvoiceID;
            partsInvoiceViewModel.ServiceInvoiceReference = salesInvoice.Reference;
            partsInvoiceViewModel.ServiceInvoiceEntryDate = salesInvoice.EntryDate;
            
            partsInvoiceViewModel.CustomerID = salesInvoice.CustomerID;
            partsInvoiceViewModel.CustomerName = salesInvoice.Customer.Name;
            partsInvoiceViewModel.CustomerBirthday = salesInvoice.Customer.Birthday;
            partsInvoiceViewModel.CustomerTelephone = salesInvoice.Customer.Telephone;
            partsInvoiceViewModel.CustomerAddressNo = salesInvoice.Customer.AddressNo;
            partsInvoiceViewModel.CustomerEntireTerritoryEntireName = salesInvoice.Customer.EntireTerritory.EntireName;

            partsInvoiceViewModel.EmployeeID = salesInvoice.EmployeeID;
            partsInvoiceViewModel.EmployeeName = salesInvoice.Employee.Name;
            partsInvoiceViewModel.PromotionID = salesInvoice.PromotionID;
            partsInvoiceViewModel.PromotionCode = salesInvoice.Promotion!= null? salesInvoice.Promotion.Code: null;
            partsInvoiceViewModel.PromotionVouchers = salesInvoice.PromotionVouchers;

            return this.CreateWizard(partsInvoiceViewModel);
        }
        


    }

}