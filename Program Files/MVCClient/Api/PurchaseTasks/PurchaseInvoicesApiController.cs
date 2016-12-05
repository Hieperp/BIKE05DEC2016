using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using MVCModel.Models;
using MVCDTO.PurchaseTasks;
using MVCCore.Repositories.PurchaseTasks;



using Microsoft.AspNet.Identity;
using MVCClient.Api.SessionTasks;




namespace MVCClient.Api.PurchaseTasks
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PurchaseInvoicesApiController : Controller
    {
        private readonly IPurchaseInvoiceRepository purchaseInvoiceRepository;
        private readonly IPurchaseInvoiceAPIRepository purchaseInvoiceAPIRepository;

        public PurchaseInvoicesApiController(IPurchaseInvoiceRepository purchaseInvoiceRepository, IPurchaseInvoiceAPIRepository purchaseInvoiceAPIRepository)
        {
            this.purchaseInvoiceRepository = purchaseInvoiceRepository;
            this.purchaseInvoiceAPIRepository = purchaseInvoiceAPIRepository;
        }

        public JsonResult GetPurchaseInvoiceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PurchaseInvoiceIndex> purchaseInvoiceIndexes = this.purchaseInvoiceAPIRepository.GetEntityIndexes<PurchaseInvoiceIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = purchaseInvoiceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int? purchaseInvoiceID, string purchaseOrderReference)
        {
            ICollection<PurchaseInvoiceGetPurchaseOrder> PurchaseInvoiceGetPurchaseOrders = this.purchaseInvoiceRepository.GetPurchaseOrders(locationID, purchaseInvoiceID, purchaseOrderReference);
            return Json(PurchaseInvoiceGetPurchaseOrders.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSuppliers([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int? purchaseInvoiceID, string supplierName)
        {
            ICollection<PurchaseInvoiceGetSupplier> PurchaseInvoiceGetSuppliers = this.purchaseInvoiceRepository.GetSuppliers(locationID, purchaseInvoiceID, supplierName);
            return Json(PurchaseInvoiceGetSuppliers.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }
}