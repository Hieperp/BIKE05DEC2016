using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCDTO.Helpers;

namespace MVCDTO.PurchaseTasks
{
    public class GoodsDetailDTO : VATAmountDetailDTO
    {
        public int SupplierID { get; set; }

        [Display(Name = "Xuất xứ")]
        public string Origin { get; set; }
        [Display(Name = "Đóng gói")]
        public string Packing { get; set; }


        [Display(Name = "Số khung")]
        public string ChassisCode { get; set; }
        [Display(Name = "Số động cơ")]
        public string EngineCode { get; set; }
        [Display(Name = "Mã màu")]
        public string ColorCode { get; set; }
    }

    public class PurchaseDetailDTO : GoodsDetailDTO
    {
        public int PurchaseOrderDetailID { get; set; }
        public int PurchaseOrderID { get; set; }
    }

    public class PurchaseOrderDetailDTO : PurchaseDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.PurchaseOrderDetailID; }

        [UIHint("NMVN/CommodityAutoComplete")]
        public override string CommodityName { get; set; }
    }
}
