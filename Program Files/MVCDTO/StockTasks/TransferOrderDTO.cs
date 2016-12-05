using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCBase.Enums;
using MVCDTO.Helpers;

namespace MVCDTO.StockTasks
{
    public abstract class TransferOrderPrimitiveDTO : QuantityDTO<TransferOrderDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.TransferOrder; } }

        public int GetID() { return this.TransferOrderID; }
        public void SetID(int id) { this.TransferOrderID = id; }

        public int TransferOrderID { get; set; }

        [Display(Name = "Ngày yêu cầu hàng về")]
        public Nullable<System.DateTime> RequestedDate { get; set; }

        public int SourceLocationID { get; set; }
        [Required]
        [Display(Name = "Chi nhánh xuất")]
        public string LocationName { get; set; }


        [Display(Name = "Loại chuyển kho")]
        public virtual int StockTransferTypeID { get; set; }

        public int WarehouseID { get; set; }
        [Required]
        [Display(Name = "Kho nhập")]
        public string WarehouseName { get; set; }
        public string WarehouseLocationTelephone { get; set; }
        public string WarehouseLocationFacsimile { get; set; }
        public string WarehouseLocationName { get; set; }
        public string WarehouseLocationAddress { get; set; }
    }



    public class VehicleTransferOrderPrimitiveDTO : TransferOrderPrimitiveDTO
    {
        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.VehicleTransferOrder; } }
        public override int StockTransferTypeID { get { return (int)GlobalEnums.StockTransferTypeID.VehicleTransfer; } }
    }

    public class VehicleTransferOrderDTO : VehicleTransferOrderPrimitiveDTO, IBaseDetailEntity<VehicleTransferOrderDetailDTO>
    {
        public VehicleTransferOrderDTO()
        {
            this.VehicleTransferOrderViewDetails = new List<VehicleTransferOrderDetailDTO>();
        }

        public List<VehicleTransferOrderDetailDTO> VehicleTransferOrderViewDetails { get; set; }
        public List<VehicleTransferOrderDetailDTO> ViewDetails { get { return this.VehicleTransferOrderViewDetails; } set { this.VehicleTransferOrderViewDetails = value; } }

        public ICollection<VehicleTransferOrderDetailDTO> GetDetails() { return this.VehicleTransferOrderViewDetails; }

        protected override IEnumerable<TransferOrderDetailDTO> DtoDetails() { return this.VehicleTransferOrderViewDetails; }
    }






    public class PartTransferOrderPrimitiveDTO : TransferOrderPrimitiveDTO
    {
        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PartTransferOrder; } }
        public override int StockTransferTypeID { get { return (int)GlobalEnums.StockTransferTypeID.PartTransfer; } }
    }

    public class PartTransferOrderDTO : PartTransferOrderPrimitiveDTO, IBaseDetailEntity<PartTransferOrderDetailDTO>
    {
        public PartTransferOrderDTO()
        {
            this.PartTransferOrderViewDetails = new List<PartTransferOrderDetailDTO>();
        }

        public List<PartTransferOrderDetailDTO> PartTransferOrderViewDetails { get; set; }
        public List<PartTransferOrderDetailDTO> ViewDetails { get { return this.PartTransferOrderViewDetails; } set { this.PartTransferOrderViewDetails = value; } }

        public ICollection<PartTransferOrderDetailDTO> GetDetails() { return this.PartTransferOrderViewDetails; }

        protected override IEnumerable<TransferOrderDetailDTO> DtoDetails() { return this.PartTransferOrderViewDetails; }
    }
}
