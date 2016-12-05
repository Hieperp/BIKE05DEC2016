using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCDTO.Helpers
{
    public interface IVATAmountDetailDTO : IAmountDetailDTO
    {
        decimal VATPercent { get; set; }
        decimal GrossPrice { get; set; }
        decimal VATAmount { get; set; }
        decimal GrossAmount { get; set; }
    }

    public abstract class VATAmountDetailDTO : AmountDetailDTO, IVATAmountDetailDTO
    {
        [Display(Name = "VAT")]
        [UIHint("DecimalReadonly")]
        public decimal VATPercent { get; set; }

        [Display(Name = "Giá sau thuế")]
        [UIHint("Decimal")]
        public virtual decimal GrossPrice { get; set; }

        [Display(Name = "Thuế VAT")]
        [UIHint("DecimalReadonly")]
        public decimal VATAmount { get; set; }

        [Display(Name = "Tổng cộng")]
        [UIHint("DecimalReadonly")]
        public decimal GrossAmount { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if ((this.UnitPrice != 0 && this.GrossPrice == 0) || (this.UnitPrice == 0 && this.GrossPrice != 0)) yield return new ValidationResult("Lỗi giá sau thuế", new[] { "GrossPrice" });
            if (Math.Round(this.Quantity * this.GrossPrice, 0) != this.GrossAmount) yield return new ValidationResult("Lỗi thành tiền sau thuế", new[] { "GrossAmount" });
            if ((this.Amount == 0 && this.VATAmount != 0) || (this.Amount != 0 && this.VATPercent != 0 && this.VATAmount == 0) || (this.Amount != 0 && this.VATPercent == 0 && this.VATAmount != 0)) yield return new ValidationResult("Lỗi tiền thuế", new[] { "VATAmount" });
            if (Math.Round(this.Amount + this.VATAmount, 0) != this.GrossAmount) yield return new ValidationResult("Lỗi thành tiền sau thuế", new[] { "GrossAmount" });
        }
    }
}
