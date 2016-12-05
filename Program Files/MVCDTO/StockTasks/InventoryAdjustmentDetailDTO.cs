using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCModel.Helpers;
using MVCDTO.Helpers;

namespace MVCDTO.StockTasks
{
    public abstract class InventoryAdjustmentDetailDTO : VATAmountDetailDTO, IPrimitiveEntity, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.InventoryAdjustmentDetailID; }

        public int InventoryAdjustmentDetailID { get; set; }
        public int InventoryAdjustmentID { get; set; }

        public int SupplierID { get; set; }
        public int InventoryAdjustmentTypeID { get; set; }

        [Range(1, 99999999999, ErrorMessage = "Lỗi bắt buộc phải có id kho")]
        public int WarehouseID { get; set; }
        public int GetWarehouseID() { return this.WarehouseID; } //Purpose: for IHelperWarehouseID only

        [Display(Name = "Kho")]
        [UIHint("StringReadonly")]
        [Required(ErrorMessage = "Vui lòng chọn kho")]
        public string WarehouseCode { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityAvailable { get; set; }

        [Range(-99999999, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        [UIHint("DecimalWithMinus")]
        public override decimal Quantity { get; set; }
    }


    public class VehicleAdjustmentDetailDTO : InventoryAdjustmentDetailDTO
    {
        public int GoodsReceiptDetailID { get; set; }
        public DateTime GoodsReceiptDate { get; set; }

        [Display(Name = "Số khung")]
        [UIHint("StringReadonly")]
        public string ChassisCode { get; set; }
        [UIHint("StringReadonly")]
        [Display(Name = "Số động cơ")]
        public string EngineCode { get; set; }
        [UIHint("StringReadonly")]
        [Display(Name = "Mã màu")]
        public string ColorCode { get; set; }

        [UIHint("NMVN/CommoditiesInGoodsReceiptsAutoComplete")]
        public override string CommodityName { get; set; }
    }










    public class PartAdjustmentDetailDTO : InventoryAdjustmentDetailDTO
    {
        [UIHint("NMVN/CommoditiesInWarehousesIncludeOutOfStockAutoComplete")]
        public override string CommodityName { get; set; }
    }
}
