using MVCModel.Models;

using MVCCore.Services.CommonTasks;
using MVCDTO.CommonTasks;
using MVCClient.ViewModels.CommonTasks;
using MVCClient.Builders.CommonTasks;

namespace MVCClient.Controllers.SalesTasks
{
    public class CommoditiesController : GenericSimpleController<Commodity, CommodityDTO, CommodityPrimitiveDTO, CommodityViewModel>
    {
        public CommoditiesController(ICommodityService commodityService, ICommodityViewModelSelectListBuilder commodityViewModelSelectListBuilder)
            : base(commodityService, commodityViewModelSelectListBuilder)
        {
        }     

    }
}