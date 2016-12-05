using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCDTO.Helpers;

namespace MVCDTO.StockTasks
{
    public abstract class TransferOrderDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.TransferOrderDetailID; }

        public int TransferOrderDetailID { get; set; }
        public int TransferOrderID { get; set; }

        public int WarehouseID { get; set; }
        [Display(Name = "Kho xuất")]
        [Required(ErrorMessage = "Vui lòng chọn kho")]
        [UIHint("StringReadonly")]
        public string WarehouseCode { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityAvailable { get; set; }
    }

    public class VehicleTransferOrderDetailDTO : TransferOrderDetailDTO
    {
        [UIHint("NMVN/VehicleAvailablesAutoComplete")]
        public override string CommodityName { get; set; }
    }

    public class PartTransferOrderDetailDTO : TransferOrderDetailDTO
    {
        [UIHint("NMVN/PartAvailablesAutoComplete")]
        public override string CommodityName { get; set; }
    }

}
