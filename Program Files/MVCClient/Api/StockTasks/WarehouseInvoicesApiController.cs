using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.UI;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using MVCBase.Enums;
using MVCModel.Models;

using MVCDTO.StockTasks;

using MVCCore.Repositories.StockTasks;
using MVCClient.ViewModels.StockTasks;
using MVCClient.Api.SessionTasks;

using Microsoft.AspNet.Identity;



namespace MVCClient.Api.StockTasks
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class WarehouseInvoiceApiController : Controller
    {
        private readonly IWarehouseInvoiceAPIRepository warehouseInvoiceAPIRepository;

        public WarehouseInvoiceApiController(IWarehouseInvoiceAPIRepository warehouseInvoiceAPIRepository)
        {
            this.warehouseInvoiceAPIRepository = warehouseInvoiceAPIRepository;
        }

        public JsonResult GetWarehouseInvoiceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<WarehouseInvoiceIndex> warehouseInvoiceIndexes = this.warehouseInvoiceAPIRepository.GetEntityIndexes<WarehouseInvoiceIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = warehouseInvoiceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPendingStockTransfers([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID)
        {
            var result = this.warehouseInvoiceAPIRepository.GetPendingStockTransfers(User.Identity.GetUserId(), locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingStockTransferDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int stockTransferID, int locationID, int sourceWarehouseID, int warehouseID, int stockTransferTypeID, DateTime fromDate, DateTime toDate, int warehouseInvoiceID, string stockTransferDetailIDs)
        {
            var result = this.warehouseInvoiceAPIRepository.GetPendingStockTransferDetails(stockTransferID, User.Identity.GetUserId(), locationID, sourceWarehouseID, warehouseID, stockTransferTypeID, fromDate, toDate.AddHours(23).AddMinutes(59).AddSeconds(59), warehouseInvoiceID, stockTransferDetailIDs);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


    }

}