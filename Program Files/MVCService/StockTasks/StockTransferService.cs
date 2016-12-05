using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCModel.Models;
using MVCDTO.SalesTasks;
using MVCCore.Repositories.CommonTasks;
using MVCCore.Services;
using MVCDTO.StockTasks;
using MVCCore.Repositories.StockTasks;
using MVCCore.Services.StockTasks;
using MVCService.Helpers;


namespace MVCService.StockTasks
{
    public class VehicleTransferService : GenericWithViewDetailService<StockTransfer, StockTransferDetail, VehicleTransferViewDetail, VehicleTransferDTO, VehicleTransferPrimitiveDTO, VehicleTransferDetailDTO>, IVehicleTransferService
    {
        public VehicleTransferService(IVehicleTransferRepository vehicleTransferRepository)
            : base(vehicleTransferRepository, "VehicleTransferPostSaveValidate", "VehicleTransferSaveRelative", null, "GetVehicleTransferViewDetails")
        {
        }

        public override ICollection<VehicleTransferViewDetail> GetViewDetails(int saleTransferID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("StockTransferID", saleTransferID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(VehicleTransferDTO vehicleTransferDTO)
        {
            vehicleTransferDTO.VehicleTransferViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(vehicleTransferDTO);
        }
    }












    public class PartTransferService : GenericWithViewDetailService<StockTransfer, StockTransferDetail, PartTransferViewDetail, PartTransferDTO, PartTransferPrimitiveDTO, PartTransferDetailDTO>, IPartTransferService
    {
        private DateTime? checkedDate; //For check over stock
        private string warehouseIDList = "";
        private string commodityIDList = "";

        private readonly IInventoryRepository inventoryRepository;
        private readonly IPartTransferHelperService partTransferHelperService;

        public PartTransferService(IPartTransferRepository partTransferRepository, IInventoryRepository inventoryRepository, IPartTransferHelperService partTransferHelperService)
            : base(partTransferRepository, "PartTransferPostSaveValidate", "PartTransferSaveRelative", null, "GetPartTransferViewDetails")
        {
            this.inventoryRepository = inventoryRepository;
            this.partTransferHelperService = partTransferHelperService;
        }

        public override ICollection<PartTransferViewDetail> GetViewDetails(int saleTransferID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("StockTransferID", saleTransferID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(PartTransferDTO partTransferDTO)
        {
            partTransferDTO.PartTransferViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(partTransferDTO);
        }

        protected override void UpdateDetail(PartTransferDTO dto, StockTransfer entity)
        {
            this.partTransferHelperService.GetWCParameters(dto, null, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UpdateDetail(dto, entity);
        }

        protected override void UndoDetail(PartTransferDTO dto, StockTransfer entity, bool isDelete)
        {
            this.partTransferHelperService.GetWCParameters(null, entity, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UndoDetail(dto, entity, isDelete);
        }

        protected override void PostSaveValidate(StockTransfer entity)
        {
            this.inventoryRepository.CheckOverStock(this.checkedDate, this.warehouseIDList, this.commodityIDList);
            base.PostSaveValidate(entity);
        }

    }

    public class PartTransferHelperService : HelperService<StockTransfer, StockTransferDetail, PartTransferDTO, PartTransferDetailDTO>, IPartTransferHelperService
    {
    }

}
