using System;
using System.Linq;
using System.Web.Mvc;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using MVCModel.Models;

using MVCDTO.PurchaseTasks;

using MVCCore.Repositories.PurchaseTasks;
using MVCClient.ViewModels.PurchaseTasks;
using System.Collections.Generic;
using AutoMapper;




using Microsoft.AspNet.Identity;
using MVCClient.Api.SessionTasks;



namespace MVCClient.Api.PurchaseTasks
{
    //[GenericSimpleAuthorizeAttribute]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PurchaseOrdersApiController : Controller
    {
        private readonly IPurchaseOrderAPIRepository purchaseOrderAPIRepository;

        public PurchaseOrdersApiController(IPurchaseOrderAPIRepository purchaseOrderAPIRepository)
        {
            this.purchaseOrderAPIRepository = purchaseOrderAPIRepository;
        }

        public JsonResult GetPurchaseOrderIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PurchaseOrderIndex> purchaseOrderIndexes = this.purchaseOrderAPIRepository.GetEntityIndexes<PurchaseOrderIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = purchaseOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}