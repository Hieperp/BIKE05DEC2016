using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCModel.Helpers;
using MVCDTO.Helpers;

namespace MVCDTO.SalesTasks
{
    public class AccountInvoiceDetailDTO : DiscountVATAmountDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.AccountInvoiceDetailID; }

        public int AccountInvoiceDetailID { get; set; }
        public int AccountInvoiceID { get; set; }

        public int CustomerID { get; set; }
        [Range(1, 99999999999, ErrorMessage = "Lỗi bắt buộc phải có id hóa đơn bán hàng")]
        public int SalesInvoiceDetailID { get; set; }

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
