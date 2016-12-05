using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using MVCBase.Enums;
using MVCModel.Models;
using MVCDTO.StockTasks;
using MVCCore.Repositories.StockTasks;




using Microsoft.AspNet.Identity;
using MVCClient.Api.SessionTasks;




namespace MVCClient.Api.StockTasks
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class TransferOrdersApiController : Controller
    {
        private readonly ITransferOrderRepository transferOrderRepository;

        public TransferOrdersApiController(ITransferOrderRepository transferOrderRepository)
        {
            this.transferOrderRepository = transferOrderRepository;
        }
    }

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class VehicleTransferOrdersApiController : TransferOrdersApiController
    {
        private readonly IVehicleTransferOrderRepository vehicleTransferOrderRepository;
        private readonly IVehicleTransferOrderAPIRepository vehicleTransferOrderAPIRepository;

        public VehicleTransferOrdersApiController(IVehicleTransferOrderRepository vehicleTransferOrderRepository, IVehicleTransferOrderAPIRepository vehicleTransferOrderAPIRepository)
            : base(vehicleTransferOrderRepository)
        {
            this.vehicleTransferOrderRepository = vehicleTransferOrderRepository;
            this.vehicleTransferOrderAPIRepository = vehicleTransferOrderAPIRepository;
        }


        public JsonResult GetVehicleTransferOrderIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<VehicleTransferOrderIndex> vehicleTransferOrderIndexes = this.vehicleTransferOrderAPIRepository.GetEntityIndexes<VehicleTransferOrderIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = vehicleTransferOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        


        /// <summary>
        /// This function should improve, since the TransferOrder has been change since 12-nov to break into 2: VehicleTransferOrder and PartTransferOrder
        /// And then: we have TransferOrders.StockTransferTypeID => so we should change this function to filter data base on this new property StockTransferTypeID, instead of commodityTypeIDList
        /// In brief: This function should be improved later!!!
        /// </summary>
        /// <param name="dataSourceRequest"></param>
        /// <param name="locationID"></param>
        /// <param name="commodityTypeIDList"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public JsonResult SearchTransferOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, string commodityTypeIDList, string searchText)
        {
            var result = vehicleTransferOrderRepository.SearchTransferOrders(locationID, commodityTypeIDList, searchText).Select(s => new
            {
                s.TransferOrderID,
                TransferOrderReference = s.Reference,
                TransferOrderEntryDate = s.EntryDate,
                TransferOrderRequestedDate = s.RequestedDate,

                s.WarehouseID,
                WarehouseCode = s.Warehouse.Code,
                WarehouseName = s.Warehouse.Name,
                WarehouseLocationName = s.Warehouse.Location.Name,
                WarehouseLocationTelephone = s.Warehouse.Location.Telephone,
                WarehouseLocationFacsimile = s.Warehouse.Location.Facsimile,
                WarehouseLocationAddress = s.Warehouse.Location.Address
            });
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }



    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PartTransferOrdersApiController : TransferOrdersApiController
    {
        private readonly IPartTransferOrderAPIRepository partTransferOrderAPIRepository;

        public PartTransferOrdersApiController(IPartTransferOrderRepository partTransferOrderRepository, IPartTransferOrderAPIRepository partTransferOrderAPIRepository)
            : base(partTransferOrderRepository)
        {
            this.partTransferOrderAPIRepository = partTransferOrderAPIRepository;
        }

        public JsonResult GetPartTransferOrderIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PartTransferOrderIndex> partTransferOrderIndexes = this.partTransferOrderAPIRepository.GetEntityIndexes<PartTransferOrderIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = partTransferOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
    }
}