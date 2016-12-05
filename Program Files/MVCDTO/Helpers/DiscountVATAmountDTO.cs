﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCDTO.Helpers
{
    public abstract class DiscountVATAmountDTO<TDiscountVATAmountDetailDTO> : VATAmountDTO<TDiscountVATAmountDetailDTO>
        where TDiscountVATAmountDetailDTO : class, IDiscountVATAmountDetailDTO
    {
        [Display(Name = "Bình quân CK")]
        public decimal AverageDiscountPercent { get; set; }
    }
}
