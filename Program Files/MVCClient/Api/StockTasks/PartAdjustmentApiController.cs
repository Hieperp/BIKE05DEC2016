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

//using MVCDTO.StockTasks;

using MVCCore.Repositories.StockTasks;
//using MVCClient.ViewModels.StockTasks;
using MVCClient.Api.SessionTasks;

using Microsoft.AspNet.Identity;


namespace MVCClient.Api.StockTasks
{
    
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PartAdjustmentApiController : Controller
    {
        private readonly IPartAdjustmentAPIRepository PartAdjustmentAPIRepository;

        public PartAdjustmentApiController(IPartAdjustmentAPIRepository PartAdjustmentAPIRepository)
        {
            this.PartAdjustmentAPIRepository = PartAdjustmentAPIRepository;
        }


        public JsonResult GetPartAdjustmentIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PartAdjustmentIndex> PartAdjustmentIndexes = this.PartAdjustmentAPIRepository.GetEntityIndexes<PartAdjustmentIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = PartAdjustmentIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }



}