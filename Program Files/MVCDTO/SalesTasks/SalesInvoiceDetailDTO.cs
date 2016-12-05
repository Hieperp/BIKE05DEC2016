using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCModel.Helpers;
using MVCDTO.Helpers;

namespace MVCDTO.SalesTasks
{
    public abstract class SalesInvoiceDetailDTO : DiscountVATAmountDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.SalesInvoiceDetailID; }

        public int SalesInvoiceDetailID { get; set; }
        public int SalesInvoiceID { get; set; }

        public int CustomerID { get; set; }
        public int SalesInvoiceTypeID { get; set; }
        public Nullable<int> ServiceContractID { get; set; }

        public Nullable<int> QuotationDetailID { get; set; }

        public Nullable<int> PromotionID { get; set; }

        public int EmployeeID { get; set; }

        public Nullable<bool> IsBonus { get; set; }
        public Nullable<bool> IsWarrantyClaim { get; set; }
    }


    public abstract class StockableInvoiceDetailDTO : SalesInvoiceDetailDTO, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int WarehouseID { get; set; }
        public int GetWarehouseID() { return this.WarehouseID; } //Purpose: for IHelperWarehouseID only

        [Display(Name = "Kho")]
        [UIHint("StringReadonly")]
        public string WarehouseCode { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityAvailable { get; set; }

        [GenericCompare(CompareToPropertyName = "QuantityAvailable", OperatorName = GenericCompareOperator.LessThanOrEqual, ErrorMessage = "Số lượng không được lớn hơn số lượng còn lại")]
        public override decimal Quantity { get; set; }
    }








    public class VehiclesInvoiceDetailDTO : StockableInvoiceDetailDTO
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










    public class PartsInvoiceDetailDTO : StockableInvoiceDetailDTO
    {
        public Nullable<int> ServiceInvoiceID { get; set; }

        [UIHint("NMVN/CommoditiesInWarehousesAutoComplete")]
        public override string CommodityName { get; set; }
    }








    public class ServicesInvoiceDetailDTO : SalesInvoiceDetailDTO
    {
        [UIHint("NMVN/CommodityAutoComplete")]
        public override string CommodityName { get; set; }

        public int CurrentMeters { get; set; }
    }
}
