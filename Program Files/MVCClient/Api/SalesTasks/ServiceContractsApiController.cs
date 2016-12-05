using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

using MVCCore.Repositories.SalesTasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using MVCDTO.SalesTasks;
using System.Collections.Generic;
using MVCModel.Models;



using Microsoft.AspNet.Identity;
using System;
using MVCClient.Api.SessionTasks;




namespace MVCClient.Api.SalesTasks
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ServiceContractsApiController : Controller
    {
        private readonly IServiceContractRepository serviceContractRepository;
        private readonly IServiceContractAPIRepository serviceContractAPIRepository;

        public ServiceContractsApiController(IServiceContractRepository serviceContractRepository, IServiceContractAPIRepository serviceContractAPIRepository)
        {
            this.serviceContractRepository = serviceContractRepository;
            this.serviceContractAPIRepository = serviceContractAPIRepository;
        }



        public JsonResult SearchAgentName(string agentName)
        {
            return Json(serviceContractRepository.SearchAgentName(agentName), JsonRequestBehavior.AllowGet);
        }


        public JsonResult SearchServiceContracts([DataSourceRequest] DataSourceRequest dataSourceRequest, string searchText)
        {
            if (searchText == "") return Json(null);

            var result = serviceContractRepository.SearchServiceContracts(searchText);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceContractIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<ServiceContractIndex> serviceContractIndexes = this.serviceContractAPIRepository.GetEntityIndexes<ServiceContractIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = serviceContractIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ServiceContractGetVehiclesInvoice([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, string searchText, int? salesInvoiceID, int? serviceContractID)
        {
            ICollection<ServiceContractGetVehiclesInvoice> serviceContractGetVehiclesInvoice = this.serviceContractRepository.ServiceContractGetVehiclesInvoice(locationID, searchText, salesInvoiceID, serviceContractID);
            return Json(serviceContractGetVehiclesInvoice.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}