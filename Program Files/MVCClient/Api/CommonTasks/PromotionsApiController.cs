using System.Linq;
using System.Web.Mvc;

using MVCCore.Repositories.CommonTasks;

namespace MVCClient.Api.CommonTasks
{
    public class PromotionsApiController : Controller
    {
        private readonly IPromotionRepository promotionRepository;

        public PromotionsApiController(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
        }

        public JsonResult SearchPromotions(int? locationID, string searchText)
        {
            var result = promotionRepository.SearchPromotions(locationID, searchText).Select(s => new { s.PromotionID, s.Code });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}