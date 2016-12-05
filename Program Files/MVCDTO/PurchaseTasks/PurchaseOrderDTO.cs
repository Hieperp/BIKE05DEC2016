using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCBase.Enums;
using MVCDTO.Helpers;

namespace MVCDTO.PurchaseTasks
{
    public abstract class PurchasePrimitiveDTO : VATAmountDTO<PurchaseDetailDTO>
    {
        public int SupplierID { get; set; }
        [Display(Name = "Nhà cung cấp")]
        [Required(ErrorMessage = "Vui lòng nhập nhà cung cấp")]
        public string CustomerName { get; set; }
        [Display(Name = "Người liên hệ")]
        public string CustomerAttentionName { get; set; }
        [Display(Name = "Địa chỉ")]
        public string CustomerAddressNo { get; set; }
        [Display(Name = "Khu vực")]
        public string CustomerEntireTerritoryEntireName { get; set; }
        [Display(Name = "Điện thoại")]
        public string CustomerTelephone { get; set; }

        [Display(Name = "Người liên hệ")]
        public string AttentionName { get; set; }
        [Display(Name = "Loại giá")]
        [Required]
        public int PriceTermID { get; set; }
        [Display(Name = "Phương thức thanh toán")]
        [Required]
        public int PaymentTermID { get; set; }
        
        
        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.SupplierID = this.SupplierID; });
        }

    }

    public class PurchaseOrderPrimitiveDTO : PurchasePrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PurchaseOrder; } }

        public int GetID() { return this.PurchaseOrderID; }
        public void SetID(int id) { this.PurchaseOrderID = id; }

        public int PurchaseOrderID { get; set; }

        [Display(Name = "Ngày xác nhận")]
        public Nullable<System.DateTime> ConfirmDate { get; set; }

        [Display(Name = "Số phiếu xác nhận")]
        public string ConfirmReference { get; set; }

    }

    public class PurchaseOrderDTO : PurchaseOrderPrimitiveDTO, IBaseDetailEntity<PurchaseOrderDetailDTO>
    {
        public PurchaseOrderDTO()
        {
            this.PurchaseOrderDetails = new List<PurchaseOrderDetailDTO>();
        }


        public List<PurchaseOrderDetailDTO> PurchaseOrderDetails { get; set; }

        public ICollection<PurchaseOrderDetailDTO> GetDetails() { return this.PurchaseOrderDetails; }

        protected override IEnumerable<PurchaseDetailDTO> DtoDetails() { return this.PurchaseOrderDetails; }
    }
}
