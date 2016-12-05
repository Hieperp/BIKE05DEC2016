using System.Linq;
using System.Web.Mvc;

using MVCCore.Repositories.CommonTasks;

namespace MVCClient.Api.CommonTasks
{
    public class WarehousesApiController : Controller
    {
        private readonly IWarehouseRepository warehouseRepository;

        public WarehousesApiController(IWarehouseRepository warehouseRepository)
        {
            this.warehouseRepository = warehouseRepository;            
        }


        public JsonResult GetAllWarehouses()
        {
            var result = warehouseRepository.GetAllWarehouses().Select(s => new { s.WarehouseID, s.Name });            

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchWarehousesByName(int? locationID, string searchText)
        {
            var result = warehouseRepository.SearchWarehousesByName(locationID, searchText).Select(s => new { s.WarehouseID, s.Name, s.Code, s.Remarks, LocationTelephone = s.Location.Telephone, LocationFacsimile = s.Location.Facsimile, LocationName = s.Location.Name, LocationAddress = s.Location.Address });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}