using MVCModel.Models;
using MVCDTO.CommonTasks;

namespace MVCCore.Services.CommonTasks
{
    public interface ICommodityService : IGenericService<Commodity, CommodityDTO, CommodityPrimitiveDTO>
    {
    }
}
