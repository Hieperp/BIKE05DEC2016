using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCModel.Helpers;
using MVCDTO.PurchaseTasks;

namespace MVCDTO.StockTasks
{
    public class GoodsReceiptDetailDTO : GoodsDetailDTO, IPrimitiveEntity, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.GoodsReceiptDetailID; }

        public int GoodsReceiptDetailID { get; set; }
        public int GoodsReceiptID { get; set; }

        public int GoodsReceiptTypeID { get; set; }

        public int VoucherDetailID { get; set; }
        public int VoucherID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn kho")]        
        public int WarehouseID { get; set; }
        public int GetWarehouseID() { return this.WarehouseID; } //Purpose: for IHelperWarehouseID only

        [Display(Name = "Kho")]
        [Required(ErrorMessage = "Vui lòng chọn kho")]
        [UIHint("NMVN/WarehouseAutoComplete")]
        public string WarehouseCode { get; set; }


        
        [Display(Name = "SL còn")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }
        
        [GenericCompare(CompareToPropertyName = "QuantityRemains", OperatorName = GenericCompareOperator.LessThanOrEqual, ErrorMessage = "Số lượng không được lớn hơn số lượng còn lại")]
        public override decimal Quantity { get; set; }
    }
}
