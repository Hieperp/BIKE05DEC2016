﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

using MVCBase.Enums;
using MVCCore.Repositories.CommonTasks;
using MVCDTO.CommonTasks;
using MVCData.Repositories;
using MVCService.CommonTasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using MVCModel.Models;
using System.Collections.Generic;



namespace MVCClient.Api.CommonTasks
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CommoditiesApiController : Controller
    {
        private readonly ICommodityRepository commodityRepository;

        public CommoditiesApiController(ICommodityRepository commodityRepository)
        {
            this.commodityRepository = commodityRepository;
        }


        /// <summary>
        /// This function is designed to use by Purchase Order import function only
        /// Never to use by orther area
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCommoditiesByCode(string code, string name, string originalName, int commodityTypeID, int commodityCategoryID)
        {
            try
            {
                var commodityResult = new { CommodityID = 0, Code = "", Name = "", CommodityTypeID = 0, VATPercent = new decimal(0) };

                var result = commodityRepository.SearchCommoditiesByName(code, null, true).Select(s => new { s.CommodityID, s.Code, s.Name, s.CommodityTypeID, s.CommodityCategory.VATPercent });
                if (result.Count() > 0)
                    commodityResult = new { CommodityID = result.First().CommodityID, Code = result.First().Code, Name = result.First().Name, CommodityTypeID = result.First().CommodityTypeID, VATPercent = result.First().VATPercent };
                else
                {
                    CommodityDTO commodityDTO = new CommodityDTO();
                    commodityDTO.Code = MVCBase.CommonExpressions.ComposeCommodityCode(code, commodityTypeID);
                    commodityDTO.Name = name;
                    commodityDTO.OfficialName = name;
                    commodityDTO.OriginalName = originalName;
                    commodityDTO.CommodityTypeID = commodityTypeID;
                    commodityDTO.CommodityCategoryID = commodityCategoryID;

                    CommodityService commodityService = new CommodityService(this.commodityRepository);
                    commodityService.UserID = 2; //Ai cung co quyen add Commodity, boi viec add can cu theo UserID = 2: tanthanhhotel@gmail.com

                    commodityDTO.PreparedPersonID = commodityService.UserID;

                    if (commodityService.Save(commodityDTO))
                        commodityResult = new { CommodityID = commodityDTO.CommodityID, Code = commodityDTO.Code, Name = commodityDTO.Name, CommodityTypeID = commodityDTO.CommodityTypeID, VATPercent = new decimal(10) };
                }

                return Json(commodityResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { CommodityID = 0, Code = ex.Message, Name = ex.Message, VATPercent = new decimal(10) }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SearchCommoditiesByName(string searchText, string commodityTypeIDList, bool? isOnlyAlphaNumericString)
        {
            var result = commodityRepository.SearchCommoditiesByName(searchText, commodityTypeIDList, isOnlyAlphaNumericString).Select(s => new { s.CommodityID, s.Code, s.Name, s.CommodityTypeID, CommodityCategoryLimitedKilometreWarranty = s.CommodityCategory.LimitedKilometreWarranty, CommodityCategoryLimitedMonthWarranty = s.CommodityCategory.LimitedMonthWarranty, s.GrossPrice, s.CommodityCategory.VATPercent });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetCommoditiesInGoodsReceipts(int? locationID, string searchText, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID)
        {
            var result = commodityRepository.GetCommoditiesInGoodsReceipts(locationID, searchText, salesInvoiceID, stockTransferID, inventoryAdjustmentID).Select(s => new { s.GoodsReceiptDetailID, s.SupplierID, s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.ChassisCode, s.EngineCode, s.ColorCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCommoditiesInGoodsReceipts([DataSourceRequest] DataSourceRequest request, int? locationID, string searchText, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID)
        {
            if (searchText == null || searchText == "") return Json(null, JsonRequestBehavior.AllowGet);

            var result = commodityRepository.GetCommoditiesInGoodsReceipts(locationID, searchText, salesInvoiceID, stockTransferID, inventoryAdjustmentID).Select(s => new { s.GoodsReceiptDetailID, s.SupplierID, s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.ChassisCode, s.EngineCode, s.ColorCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

            DataSourceResult response = result.ToDataSourceResult(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetCommoditiesInWarehouses(int? locationID, DateTime? entryDate, string searchText, bool wholeMatch, bool includeCommoditiesOutOfStock, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID)
        {
            var result = commodityRepository.GetCommoditiesInWarehouses(locationID, entryDate, searchText, wholeMatch, includeCommoditiesOutOfStock, salesInvoiceID, stockTransferID, inventoryAdjustmentID).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetCommoditiesAvailables(int? locationID, DateTime? entryDate, string searchText, bool wholeMatch)
        {
            var result = commodityRepository.GetCommoditiesAvailables(locationID, entryDate, searchText, wholeMatch).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetVehicleAvailables(int? locationID, DateTime? entryDate, string searchText, bool wholeMatch)
        {
            var result = commodityRepository.GetVehicleAvailables(locationID, entryDate, searchText, wholeMatch).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetPartAvailables(int? locationID, DateTime? entryDate, string searchText, bool wholeMatch)
        {
            var result = commodityRepository.GetPartAvailables(locationID, entryDate, searchText, wholeMatch).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchPartAvailables([DataSourceRequest] DataSourceRequest request, int? locationID, DateTime? entryDate, string searchText, bool wholeMatch)
        {
            if (entryDate == null) entryDate = DateTime.Now;
            var result = commodityRepository.GetPartAvailables(locationID, entryDate, searchText, wholeMatch).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

            DataSourceResult response = result.ToDataSourceResult(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCommodities([DataSourceRequest] DataSourceRequest request, int commodityCategoryID, int commodityTypeID)
        {
            var commodities = this.commodityRepository.SearchCommoditiesByIndex(commodityCategoryID, commodityTypeID);

            DataSourceResult response = commodities.ToDataSourceResult(request, o => new CommodityPrimitiveDTO
            {
                CommodityID = o.CommodityID,
                Code = o.Code,
                Name = o.Name,
                OfficialName = o.OfficialName,
                OriginalName = o.OriginalName,
                //CommodityTypeName = o.CommodityType.Name,
                //CommodityCategoryName = o.CommodityCategory.Name,
                GrossPrice = o.GrossPrice,
                Remarks = o.Remarks
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }





        /// <summary>
        /// This function is designed to use by import function only
        /// Never to use by orther area
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAjaxCommoditiesInWarehouses(int? locationID, DateTime? entryDate, string searchText, bool wholeMatch, bool includeCommoditiesOutOfStock, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID)
        {
            try
            {
                var commodityResult = new { CommodityID = 0, CommodityCode = "", CommodityName = "", CommodityTypeID = 0, WarehouseID = 0, WarehouseCode = "", QuantityAvailable = new decimal(0), GrossPrice = new decimal(0), VATPercent = new decimal(0) };

                var result = commodityRepository.GetCommoditiesInWarehouses(locationID, entryDate, searchText, wholeMatch, includeCommoditiesOutOfStock, salesInvoiceID, stockTransferID, inventoryAdjustmentID).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

                if (result.Count() > 0)
                    commodityResult = new { CommodityID = result.First().CommodityID, CommodityCode = result.First().CommodityCode, CommodityName = result.First().CommodityName, CommodityTypeID = result.First().CommodityTypeID, WarehouseID = (result.First().WarehouseID == null? 0 : (int)result.First().WarehouseID), WarehouseCode = result.First().WarehouseCode, QuantityAvailable = (decimal)result.First().QuantityAvailable, GrossPrice = (decimal)result.First().GrossPrice, VATPercent = (decimal)result.First().VATPercent };

                return Json(commodityResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { CommodityID = 0, CommodityCode = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}