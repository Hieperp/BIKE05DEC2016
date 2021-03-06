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
    
    public partial class AccountInvoiceSheet
    {
        public int AccountInvoiceID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public string VATInvoiceNo { get; set; }
        public System.DateTime VATInvoiceDate { get; set; }
        public string VATInvoiceSeries { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerOfficialName { get; set; }
        public string VATCode { get; set; }
        public string AddressNo { get; set; }
        public string EntireTerritoryEntireName { get; set; }
        public int PaymentTermID { get; set; }
        public string PaymentTermName { get; set; }
        public Nullable<int> MINSalesInvoiceTypeID { get; set; }
        public int SalesInvoiceDetailID { get; set; }
        public bool IsBonus { get; set; }
        public Nullable<int> CommodityID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SalesUnit { get; set; }
        public string LineDescription { get; set; }
        public string ChassisCode { get; set; }
        public string EngineCode { get; set; }
        public string ColorCode { get; set; }
        public string ColorCodeName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VATPercent { get; set; }
        public decimal Amount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalVATAmount { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public string TotalGrossAmountInWords { get; set; }
        public string Description { get; set; }
        public string LocationOfficialName { get; set; }
        public string LocationAddress { get; set; }
        public string LocationTaxcode { get; set; }
        public string LocationTelephone { get; set; }
        public string LocationFacsimile { get; set; }
        public string Telephone { get; set; }
        public bool Approved { get; set; }
        public Nullable<int> ApiPublishID { get; set; }
        public string ApiURL { get; set; }
        public string ApiAccount { get; set; }
        public string ApiACPass { get; set; }
        public string ApiUsername { get; set; }
        public string ApiPass { get; set; }
        public string ApiPattern { get; set; }
        public string ApiSerial { get; set; }
    }
}
