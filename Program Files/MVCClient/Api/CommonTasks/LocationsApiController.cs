using System.Linq;
using System.Web.Mvc;

using MVCCore.Repositories.CommonTasks;

namespace MVCClient.Api.CommonTasks
{
    public class LocationsApiController : Controller
    {
        private readonly ILocationRepository locationRepository;

        public LocationsApiController(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        /// <summary>
        /// Until now, SearchLocationsByName is only used in TransferOrder
        /// And at now: this filterLocationID parameter is now fixed to: -1 for VehicleTransferOrder OR current logon user LocationID for PartTransferOrder
        /// this is correct for now, because: VehicleTransferOrder: is granted editable for only xe@tt1.com: who have to create/ edit all VehicleTransferOrder for all location
        /// while PartTransferOrder is now granted to all pt@xx.com of every location, for this reason: very logon user have permission to their location only
        /// </summary>
        /// <param name="filterLocationID"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public JsonResult SearchLocationsByName(int? filterLocationID, string searchText)
        {
            if (filterLocationID == -1) filterLocationID = null;

            var result = locationRepository.SearchLocationsByName(filterLocationID, searchText).Select(s => new { s.LocationID, s.Code, s.Name });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}