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
    public class WarehouseInvoicePrimitiveDTO : DiscountVATAmountDTO<WarehouseInvoiceDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.WarehouseInvoice; } }

        public int GetID() { return this.WarehouseInvoiceID; }
        public void SetID(int id) { this.WarehouseInvoiceID = id; }

        public int WarehouseInvoiceID { get; set; }

        public int WarehouseID { get; set; }
        [Display(Name = "Khách hàng")]
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string WarehouseName { get; set; }
        [Display(Name = "Ngày sinh")]
        public Nullable<System.DateTime> WarehouseBirthday { get; set; }
        [Display(Name = "Mã số thuế")]
        public string WarehouseVATCode { get; set; }
        [Display(Name = "Điện thoại")]
        public string WarehouseTelephone { get; set; }
        [Display(Name = "Địa chỉ")]
        public string WarehouseAddressNo { get; set; }
        [Display(Name = "Khu vực")]
        public string WarehouseEntireTerritoryEntireName { get; set; }

        [Display(Name = "Số hóa đơn")]
        [Required(ErrorMessage = "Vui lòng nhập Số hóa đơn")]
        public string VATInvoiceNo { get; set; }
        [Display(Name = "Số seri")]
        [Required(ErrorMessage = "Vui lòng nhập Số seri")]
        public string VATInvoiceSeries { get; set; }
        [Display(Name = "Ngày hóa đơn")]
        [Required(ErrorMessage = "Vui lòng Ngày hóa đơn")]
        public Nullable<System.DateTime> VATInvoiceDate { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public int PaymentTermID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.WarehouseID = this.WarehouseID; });
        }
    }

    public class WarehouseInvoiceDTO : WarehouseInvoicePrimitiveDTO, IBaseDetailEntity<WarehouseInvoiceDetailDTO>
    {
        public WarehouseInvoiceDTO()
        {
            this.WarehouseInvoiceViewDetails = new List<WarehouseInvoiceDetailDTO>();
        }

        public List<WarehouseInvoiceDetailDTO> WarehouseInvoiceViewDetails { get; set; }
        public List<WarehouseInvoiceDetailDTO> ViewDetails { get { return this.WarehouseInvoiceViewDetails; } set { this.WarehouseInvoiceViewDetails = value; } }

        public ICollection<WarehouseInvoiceDetailDTO> GetDetails() { return this.WarehouseInvoiceViewDetails; }

        protected override IEnumerable<WarehouseInvoiceDetailDTO> DtoDetails() { return this.WarehouseInvoiceViewDetails; }
    }

}
