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
    
    public partial class WarehouseInvoiceViewDetail
    {
        public int WarehouseInvoiceDetailID { get; set; }
        public int WarehouseInvoiceID { get; set; }
        public Nullable<int> StockTransferDetailID { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public string ChassisCode { get; set; }
        public string EngineCode { get; set; }
        public string ColorCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal ListedPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VATPercent { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> StockTransferID { get; set; }
        public Nullable<int> GoodsReceiptID { get; set; }
        public Nullable<int> GoodsReceiptDetailID { get; set; }
        public string VATInvoiceNo { get; set; }
    }
}
