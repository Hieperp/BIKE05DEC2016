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

        public int? SourceWarehouseID { get; set; }
        public int WarehouseID { get; set; }

        public int? GoodsReceiptID { get; set; }
        public int? GoodsReceiptDetailID { get; set; }

        public int? StockTransferID { get; set; }
        public int? StockTransferDetailID { get; set; }

        [UIHint("StringReadonly")]
        public override string CommodityName { get; set; }

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

        [Display(Name = "HĐ")]
        [UIHint("StringReadonly")]
        public string VATInvoiceNo { get; set; }

        [Display(Name = "Số khung")]
        [UIHint("StringReadonly")]
        public string ChassisCode { get; set; }
        [UIHint("StringReadonly")]
        [Display(Name = "Số động cơ")]
        public string EngineCode { get; set; }
        [UIHint("StringReadonly")]
        [Display(Name = "Mã màu")]
        public string ColorCode { get; set; }

        public override System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.GoodsReceiptDetailID == null && this.StockTransferDetailID == null) yield return new ValidationResult("Bắt buộc phải có dữ liệu gốc [" + this.CommodityCode + "]", new[] { "CommodityCode" });
        }
    }
}
