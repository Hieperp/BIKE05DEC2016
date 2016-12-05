using System;
using System.Web.UI;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using MVCBase.Enums;
using MVCModel.Models;

using MVCDTO.SalesTasks;

using MVCCore.Repositories.SalesTasks;
using MVCClient.ViewModels.SalesTasks;
using MVCCore.Repositories.StockTasks;
using MVCDTO.StockTasks;





using Microsoft.AspNet.Identity;
using MVCClient.Api.SessionTasks;




namespace MVCClient.Api.StockTasks
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class VehicleTransferApiController : Controller
    {
        private readonly IVehicleTransferRepository vehicleTransferRepository;
        private readonly IVehicleTransferAPIRepository vehicleTransferAPIRepository;

        public VehicleTransferApiController(IVehicleTransferRepository vehicleTransferRepository, IVehicleTransferAPIRepository vehicleTransferAPIRepository)
        {
            this.vehicleTransferRepository = vehicleTransferRepository;
            this.vehicleTransferAPIRepository = vehicleTransferAPIRepository;
        }

        public JsonResult GetVehicleTransferIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<VehicleTransferIndex> vehicleTransferIndexes = this.vehicleTransferAPIRepository.GetEntityIndexes<VehicleTransferIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = vehicleTransferIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult GetPendingVehicleTransferOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int transferOrderID)
        {
            var result = this.vehicleTransferRepository.GetPendingVehicleTransferOrders(locationID, transferOrderID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }






    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PartTransferApiController : Controller
    {
        private readonly IPartTransferRepository partTransferRepository;
        private readonly IPartTransferAPIRepository partTransferAPIRepository;

        public PartTransferApiController(IPartTransferRepository partTransferRepository, IPartTransferAPIRepository partTransferAPIRepository)
        {
            this.partTransferRepository = partTransferRepository;
            this.partTransferAPIRepository = partTransferAPIRepository;
        }

        public JsonResult GetPartTransferIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PartTransferIndex> partTransferIndexes = this.partTransferAPIRepository.GetEntityIndexes<PartTransferIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = partTransferIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPendingPartTransferOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int transferOrderID)
        {
            var result = this.partTransferRepository.GetPendingPartTransferOrders(locationID, transferOrderID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }
  
}