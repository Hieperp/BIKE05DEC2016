using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCBase.Enums;
using MVCModel.Models;
using MVCDTO.StockTasks;
using MVCCore.Repositories.StockTasks;
using MVCCore.Repositories.CommonTasks;
using MVCCore.Services.StockTasks;
using MVCCore.Services.Helpers;
using MVCService.Helpers;

namespace MVCService.StockTasks
{
    public class VehicleAdjustmentService : GenericWithViewDetailService<InventoryAdjustment, InventoryAdjustmentDetail, VehicleAdjustmentViewDetail, VehicleAdjustmentDTO, VehicleAdjustmentPrimitiveDTO, VehicleAdjustmentDetailDTO>, IVehicleAdjustmentService
    {
        public VehicleAdjustmentService(IVehicleAdjustmentRepository vehicleAdjustmentRepository)
            : base(vehicleAdjustmentRepository, "VehicleAdjustmentPostSaveValidate", "VehicleAdjustmentSaveRelative", null, "GetVehicleAdjustmentViewDetails")
        {
        }

        public override ICollection<VehicleAdjustmentViewDetail> GetViewDetails(int inventoryAdjustmentID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("InventoryAdjustmentID", inventoryAdjustmentID) };
            return this.GetViewDetails(parameters);
        }


        public override bool Save(VehicleAdjustmentDTO vehicleAdjustmentDTO)
        {
            vehicleAdjustmentDTO.VehicleAdjustmentViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(vehicleAdjustmentDTO);
        }
    }












    public class PartAdjustmentService : GenericWithViewDetailService<InventoryAdjustment, InventoryAdjustmentDetail, PartAdjustmentViewDetail, PartAdjustmentDTO, PartAdjustmentPrimitiveDTO, PartAdjustmentDetailDTO>, IPartAdjustmentService
    {
        private DateTime? checkedDate; //For check over stock
        private string warehouseIDList = "";
        private string commodityIDList = "";

        private readonly IInventoryRepository inventoryRepository;
        private readonly IPartAdjustmentHelperService partAdjustmentHelperService;


        public PartAdjustmentService(IPartAdjustmentRepository partAdjustmentRepository, IInventoryRepository inventoryRepository, IPartAdjustmentHelperService partAdjustmentHelperService)
            : base(partAdjustmentRepository, "PartAdjustmentPostSaveValidate", "PartAdjustmentSaveRelative", null, "GetPartAdjustmentViewDetails")
        {
            this.inventoryRepository = inventoryRepository;
            this.partAdjustmentHelperService = partAdjustmentHelperService;
        }

        public override ICollection<PartAdjustmentViewDetail> GetViewDetails(int inventoryAdjustmentID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("InventoryAdjustmentID", inventoryAdjustmentID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(PartAdjustmentDTO partAdjustmentDTO)
        {
            partAdjustmentDTO.PartAdjustmentViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(partAdjustmentDTO);
        }

        protected override void UpdateDetail(PartAdjustmentDTO dto, InventoryAdjustment entity)
        {
            this.partAdjustmentHelperService.GetWCParameters(dto, null, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UpdateDetail(dto, entity);
        }

        protected override void UndoDetail(PartAdjustmentDTO dto, InventoryAdjustment entity, bool isDelete)
        {
            this.partAdjustmentHelperService.GetWCParameters(null, entity, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UndoDetail(dto, entity, isDelete);
        }



        protected override void PostSaveValidate(InventoryAdjustment entity)
        {
            this.inventoryRepository.CheckOverStock(this.checkedDate, this.warehouseIDList, this.commodityIDList);
            base.PostSaveValidate(entity);
        }

    }






    






    public class PartAdjustmentHelperService : HelperService<InventoryAdjustment, InventoryAdjustmentDetail, PartAdjustmentDTO, PartAdjustmentDetailDTO>, IPartAdjustmentHelperService
    {
    }


}
