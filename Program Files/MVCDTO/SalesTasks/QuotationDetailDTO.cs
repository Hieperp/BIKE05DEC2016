using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCDTO.Helpers;

namespace MVCDTO.SalesTasks
{
    public class QuotationDetailDTO : DiscountVATAmountDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.QuotationDetailID; }

        public int QuotationDetailID { get; set; }
        public int QuotationID { get; set; }

        
        [UIHint("NMVN/CommodityAutoComplete")]
        public override string CommodityName { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityAvailable { get; set; }
        
        public Nullable<bool> IsBonus { get; set; }
        public Nullable<bool> IsWarrantyClaim { get; set; }
    }

    /// <summary>
    /// This class is used as model of kendo grid to show in the popup windows with a selected checkbox column: IsSelected
    /// </summary>
    public class QuotationDetailPopupDTO : QuotationDetailDTO
    {
        public int WarehouseID { get; set; }
        [Display(Name = "Kho")]
        [UIHint("StringReadonly")]
        public string WarehouseCode { get; set; }


        public Nullable<bool> IsSelected { get; set; }
    }
}
