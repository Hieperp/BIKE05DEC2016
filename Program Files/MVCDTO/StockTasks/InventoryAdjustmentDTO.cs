using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCBase.Enums;
using MVCDTO.Helpers;

namespace MVCDTO.StockTasks
{
    public abstract class InventoryAdjustmentPrimitiveDTO : VATAmountDTO<InventoryAdjustmentDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.InventoryAdjustment; } }

        public int GetID() { return this.InventoryAdjustmentID; }
        public void SetID(int id) { this.InventoryAdjustmentID = id; }

        public int InventoryAdjustmentID { get; set; }

        public int SupplierID { get; set; }
        [Display(Name = "Khách hàng")]
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string CustomerName { get; set; }
        [Display(Name = "Người liên hệ")]
        public string CustomerAttentionName { get; set; }
        [Display(Name = "Điện thoại")]
        public string CustomerTelephone { get; set; }
        [Display(Name = "Địa chỉ")]
        public string CustomerAddressNo { get; set; }
        [Display(Name = "Khu vực")]
        public string CustomerEntireTerritoryEntireName { get; set; }

        [Display(Name = "Loại điều chỉnh kho")]
        public virtual int InventoryAdjustmentTypeID { get; set; }
        [Display(Name = "Phương thức thanh toán")]
        public int PaymentTermID { get; set; }

        [Display(Name = "Số chứng từ")]
        [Required(ErrorMessage = "Vui lòng nhập Số biên bản điều chỉnh kho")]
        public string MemoNo { get; set; }
        [Display(Name = "Ngày chứng từ")]
        [Required(ErrorMessage = "Vui lòng Ngày biên bản điều chỉnh kho")]
        public Nullable<System.DateTime> MemoDate { get; set; }
        [Display(Name = "Lý do điều chỉnh kho")]
        public string MemoJob { get; set; }

        public int EmployeeID { get; set; }
        [Display(Name = "Nhân viên thực hiện")]
        [Required(ErrorMessage = "Vui lòng nhập tên nhân viên thực hiện")]
        public string EmployeeName { get; set; }



        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.InventoryAdjustmentTypeID = this.InventoryAdjustmentTypeID; e.SupplierID = this.SupplierID; });
        }

    }








    public class VehicleAdjustmentPrimitiveDTO : InventoryAdjustmentPrimitiveDTO
    {
        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.VehicleAdjustment; } }
        public override int InventoryAdjustmentTypeID { get { return (int)GlobalEnums.InventoryAdjustmentTypeID.VehicleAdjustment; } }
    }

    public class VehicleAdjustmentDTO : VehicleAdjustmentPrimitiveDTO, IBaseDetailEntity<VehicleAdjustmentDetailDTO>
    {
        public VehicleAdjustmentDTO()
        {
            this.VehicleAdjustmentViewDetails = new List<VehicleAdjustmentDetailDTO>();
        }

        public List<VehicleAdjustmentDetailDTO> VehicleAdjustmentViewDetails { get; set; }
        public List<VehicleAdjustmentDetailDTO> ViewDetails { get { return this.VehicleAdjustmentViewDetails; } set { this.VehicleAdjustmentViewDetails = value; } }

        public ICollection<VehicleAdjustmentDetailDTO> GetDetails() { return this.VehicleAdjustmentViewDetails; }

        protected override IEnumerable<InventoryAdjustmentDetailDTO> DtoDetails() { return this.VehicleAdjustmentViewDetails; }
    }














    public class PartAdjustmentPrimitiveDTO : InventoryAdjustmentPrimitiveDTO
    {
        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PartAdjustment; } }
        public override int InventoryAdjustmentTypeID { get { return (int)GlobalEnums.InventoryAdjustmentTypeID.PartAdjustment; } }
    }

    public class PartAdjustmentDTO : PartAdjustmentPrimitiveDTO, IBaseDetailEntity<PartAdjustmentDetailDTO>
    {
        public PartAdjustmentDTO()
        {
            this.PartAdjustmentViewDetails = new List<PartAdjustmentDetailDTO>();
        }

        public List<PartAdjustmentDetailDTO> PartAdjustmentViewDetails { get; set; }
        public List<PartAdjustmentDetailDTO> ViewDetails { get { return this.PartAdjustmentViewDetails; } set { this.PartAdjustmentViewDetails = value; } }

        public ICollection<PartAdjustmentDetailDTO> GetDetails() { return this.PartAdjustmentViewDetails; }

        protected override IEnumerable<InventoryAdjustmentDetailDTO> DtoDetails() { return this.PartAdjustmentViewDetails; }
    }
}
