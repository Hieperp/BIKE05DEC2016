using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;
using MVCDTO.CommonTasks;
using MVCClient.ViewModels.CommonTasks;

namespace MVCClient.Api.CommonTasks
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class EntireTerritoriesApiController : Controller
    {
        private readonly IEntireTerritoryRepository customerRepository;        

        public EntireTerritoriesApiController(IEntireTerritoryRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
    

        public JsonResult SearchEntireTerritoriesByName(string text)
        {
            var result = customerRepository.SearchEntireTerritoriesByName(text).Select(s => new { s.TerritoryID, s.EntireName });

            return Json(result, JsonRequestBehavior.AllowGet);
        }        
    }
}