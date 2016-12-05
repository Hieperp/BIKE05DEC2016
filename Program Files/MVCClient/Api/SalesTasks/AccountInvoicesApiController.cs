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

using MVCDTO.SalesTasks;

using MVCCore.Repositories.SalesTasks;
using MVCClient.ViewModels.SalesTasks;
using MVCClient.Api.SessionTasks;

using Microsoft.AspNet.Identity;



namespace MVCClient.Api.SalesTasks
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class AccountInvoiceApiController : Controller
    {
        private readonly IAccountInvoiceAPIRepository accountInvoiceAPIRepository;

        public AccountInvoiceApiController(IAccountInvoiceAPIRepository accountInvoiceAPIRepository)
        {
            this.accountInvoiceAPIRepository = accountInvoiceAPIRepository;
        }

        public JsonResult GetAccountInvoiceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<AccountInvoiceIndex> accountInvoiceIndexes = this.accountInvoiceAPIRepository.GetEntityIndexes<AccountInvoiceIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = accountInvoiceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPendingSalesInvoices([DataSourceRequest] DataSourceRequest dataSourceRequest, int salesInvoiceID, int locationID, int salesInvoiceTypeID, DateTime entryDate, int accountInvoiceID, string salesInvoiceDetailIDs)
        {
            var result = this.accountInvoiceAPIRepository.GetPendingSalesInvoices(salesInvoiceID, User.Identity.GetUserId(), locationID, salesInvoiceTypeID, entryDate, entryDate.AddHours(23).AddMinutes(59).AddSeconds(59), accountInvoiceID, salesInvoiceDetailIDs);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


    }

}