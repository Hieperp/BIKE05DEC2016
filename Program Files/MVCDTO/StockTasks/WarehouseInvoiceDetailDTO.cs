using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCModel.Helpers;
using MVCDTO.Helpers;

namespace MVCDTO.StockTasks
{
    public class WarehouseInvoiceDetailDTO : DiscountVATAmountDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.WarehouseInvoiceDetailID; }

        public int WarehouseInvoiceDetailID { get; set; }
        public int WarehouseInvoiceID { get; set; }

        public int WarehouseID { get; set; }
        [Range(1, 99999999999, ErrorMessage = "Lỗi bắt buộc phải có id hóa đơn bán hàng")]
        public int StockTransferDetailID { get; set; }

        [UIHint("DecimalReadonly")]
        public override decimal Quantity { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal DiscountPercent { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal UnitPrice { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal GrossPrice { get; set; }
        
        
        public Nullable<bool> IsBonus { get; set; }
        public Nullable<bool> IsWarrantyClaim { get; set; }

        [Display(Name = "Số khung")]
        [UIHint("StringReadonly")]
        public string ChassisCode { get; set; }
        [UIHint("StringReadonly")]
        [Display(Name = "Số động cơ")]
        public string EngineCode { get; set; }
        [UIHint("StringReadonly")]
        [Display(Name = "Mã màu")]
        public string ColorCode { get; set; }
    }
}
