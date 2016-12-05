using System.Collections.Generic;

using MVCModel.Models;
using MVCDTO.StockTasks;
using MVCCore.Services.Helpers;

namespace MVCCore.Services.StockTasks
{
    public interface IVehicleTransferService : IGenericWithViewDetailService<StockTransfer, StockTransferDetail, VehicleTransferViewDetail, VehicleTransferDTO, VehicleTransferPrimitiveDTO, VehicleTransferDetailDTO>
    {
    }

    public interface IPartTransferService : IGenericWithViewDetailService<StockTransfer, StockTransferDetail, PartTransferViewDetail, PartTransferDTO, PartTransferPrimitiveDTO, PartTransferDetailDTO>
    {
    }

    public interface IPartTransferHelperService : IHelperService<StockTransfer, StockTransferDetail, PartTransferDTO, PartTransferDetailDTO>
    {
    }
}

