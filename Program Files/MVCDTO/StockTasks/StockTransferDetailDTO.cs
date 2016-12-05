using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCModel.Helpers;
using MVCDTO.Helpers;

namespace MVCDTO.StockTasks
{
    public abstract class StockTransferDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.StockTransferDetailID; }

        public int StockTransferDetailID { get; set; }
        public int StockTransferID { get; set; }

        public Nullable<int> TransferOrderDetailID { get; set; }

        
        public int WarehouseID { get; set; }
        [Display(Name = "Kho xuất")]
        [UIHint("StringReadonly")]
        public string WarehouseCode { get; set; }
        [Display(Name = "Kho xuất")]
        [UIHint("StringReadonly")]
        public string WarehouseName { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityAvailable { get; set; }

        [GenericCompare(CompareToPropertyName = "QuantityAvailable", OperatorName = GenericCompareOperator.LessThanOrEqual, ErrorMessage = "Số lượng không được lớn hơn số lượng còn lại")]
        public override decimal Quantity { get; set; }
    }

    public class VehicleTransferDetailDTO : StockTransferDetailDTO
    {
        [Range(1, 99999999999, ErrorMessage = "Lỗi bắt buộc phải có id của phiếu nhập kho")]
        public int GoodsReceiptDetailID { get; set; }
        [Range(1, 99999999999, ErrorMessage = "Lỗi bắt buộc phải có id nhà cung cấp")]
        public int SupplierID { get; set; }

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




    public class PartTransferDetailDTO : StockTransferDetailDTO, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetWarehouseID() { return this.WarehouseID; } //Purpose: for IHelperWarehouseID only

        [UIHint("NMVN/CommoditiesInWarehousesAutoComplete")]
        public override string CommodityName { get; set; }
    }
}
