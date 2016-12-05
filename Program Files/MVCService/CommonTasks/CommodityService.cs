using MVCModel.Models;
using MVCDTO.CommonTasks;
using MVCCore.Repositories.CommonTasks;
using MVCCore.Services.CommonTasks;


namespace MVCService.CommonTasks
{
    public class CommodityService : GenericService<Commodity, CommodityDTO, CommodityPrimitiveDTO>, ICommodityService
    {
        public CommodityService(ICommodityRepository commodityRepository)
            : base(commodityRepository)
        {
        }

    }
}

