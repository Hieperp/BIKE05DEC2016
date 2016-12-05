using System;
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



namespace MVCClient.Api.CommonTasks
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CustomerTypesApiController : Controller
    {
        private readonly ICustomerTypeRepository commodityTypeRepository;

        public CustomerTypesApiController(ICustomerTypeRepository commodityTypeRepository)
        {
            this.commodityTypeRepository = commodityTypeRepository;
        }

        /// <summary>
        /// This function is designed to use by Purchase Order import function only
        /// Never to use by orther area
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetAllCustomerTypes()
        {
            var result = commodityTypeRepository.GetAllCustomerTypes().Select(s => new { s.CustomerTypeID, s.Name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CustomerTypesForTreeView(int? id)
        {
            var commodityCategories = this.commodityTypeRepository.CustomerTypesForTreeView(id);

            return Json(commodityCategories, JsonRequestBehavior.AllowGet);
        }
    }
}