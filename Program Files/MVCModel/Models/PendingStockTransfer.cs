//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCModel.Models
{
    using System;
    
    public partial class PendingStockTransfer
    {
        public int StockTransferDetailID { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public string WarehouseName { get; set; }
        public string ChassisCode { get; set; }
        public string EngineCode { get; set; }
        public string ColorCode { get; set; }
        public decimal Quantity { get; set; }
        public int ListedPrice { get; set; }
        public int DiscountPercent { get; set; }
        public int UnitPrice { get; set; }
        public int VATPercent { get; set; }
        public int GrossPrice { get; set; }
        public int Amount { get; set; }
        public int VATAmount { get; set; }
        public int GrossAmount { get; set; }
        public Nullable<bool> IsSelected { get; set; }
    }
}
