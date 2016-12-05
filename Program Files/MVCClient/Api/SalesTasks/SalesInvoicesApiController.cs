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
    public class VehiclesInvoiceApiController : Controller
    {
        private readonly IVehiclesInvoiceAPIRepository vehiclesInvoiceAPIRepository;

        public VehiclesInvoiceApiController(IVehiclesInvoiceAPIRepository vehiclesInvoiceAPIRepository)
        {
            this.vehiclesInvoiceAPIRepository = vehiclesInvoiceAPIRepository;
        }

        public JsonResult GetVehiclesInvoiceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<VehiclesInvoiceIndex> vehiclesInvoiceIndexes = this.vehiclesInvoiceAPIRepository.GetEntityIndexes<VehiclesInvoiceIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = vehiclesInvoiceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }



    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PartsInvoiceApiController : Controller
    {
        private readonly IPartsInvoiceAPIRepository partsInvoiceAPIRepository;

        public PartsInvoiceApiController(IPartsInvoiceAPIRepository partsInvoiceAPIRepository)
        {
            this.partsInvoiceAPIRepository = partsInvoiceAPIRepository;
        }


        public JsonResult GetPartsInvoiceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PartsInvoiceIndex> partsInvoiceIndexes = this.partsInvoiceAPIRepository.GetEntityIndexes<PartsInvoiceIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = partsInvoiceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }






    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ServicesInvoiceApiController : Controller
    {
        private readonly IServicesInvoiceRepository servicesInvoiceRepository;
        private readonly IServicesInvoiceAPIRepository servicesInvoiceAPIRepository;

        public ServicesInvoiceApiController(IServicesInvoiceRepository servicesInvoiceRepository, IServicesInvoiceAPIRepository servicesInvoiceAPIRepository)
        {
            this.servicesInvoiceRepository = servicesInvoiceRepository;
            this.servicesInvoiceAPIRepository = servicesInvoiceAPIRepository;
        }


        public JsonResult GetServicesInvoiceIndexes([DataSourceRequest] DataSourceRequest request, int? serviceContractID)
        {
            ICollection<ServicesInvoiceIndex> servicesInvoiceIndexes = serviceContractID == null || serviceContractID == -1 ? this.servicesInvoiceAPIRepository.GetEntityIndexes<ServicesInvoiceIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext)) : this.servicesInvoiceAPIRepository.SearchServicesInvoiceIndexes((int)serviceContractID);

            DataSourceResult response = servicesInvoiceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetActiveServiceInvoices([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int? serviceInvoiceID, string searchText, int isFinished)
        {
            if (searchText == "") return Json(null);

            var result = servicesInvoiceRepository.GetActiveServiceInvoices(locationID, serviceInvoiceID, searchText, isFinished);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 0)]
        public JsonResult GetRelatedPartsInvoiceValue(int serviceInvoiceID)
        {
            try
            {
                var relatedPartsInvoiceValue = this.servicesInvoiceRepository.GetRelatedPartsInvoiceValue(serviceInvoiceID);
                return Json(new
                {
                    NoInvoice = relatedPartsInvoiceValue[0].NoInvoice,
                    MinSalesInvoiceID = relatedPartsInvoiceValue[0].MinSalesInvoiceID,
                    MinReference = relatedPartsInvoiceValue[0].MinReference,
                    TotalQuantity = relatedPartsInvoiceValue[0].TotalQuantity,
                    TotalPartsAmount = relatedPartsInvoiceValue[0].TotalGrossAmount                    
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}