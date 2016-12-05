using System.ComponentModel.DataAnnotations;

namespace MVCDTO.Helpers
{
    public interface IDiscountVATAmountDetailDTO : IVATAmountDetailDTO
    {
        decimal ListedPrice { get; set; }
        decimal DiscountPercent { get; set; }
    }

    public abstract class DiscountVATAmountDetailDTO : VATAmountDetailDTO, IDiscountVATAmountDetailDTO
    {
        [Display(Name = "Giá niêm yết")]
        [UIHint("DecimalReadonly")]
        public decimal ListedPrice { get; set; }

        [Display(Name = "CK")]
        [UIHint("Decimal")]
        public virtual decimal DiscountPercent { get; set; }
    }
}
