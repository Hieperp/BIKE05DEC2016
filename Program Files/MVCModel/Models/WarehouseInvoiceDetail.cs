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
    using System.Collections.Generic;
    
    public partial class WarehouseInvoiceDetail
    {
        public int WarehouseInvoiceDetailID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public int LocationID { get; set; }
        public int WarehouseInvoiceID { get; set; }
        public int WarehouseID { get; set; }
        public int StockTransferDetailID { get; set; }
        public string Remarks { get; set; }
    
        public virtual StockTransferDetail StockTransferDetail { get; set; }
        public virtual WarehouseInvoice WarehouseInvoice { get; set; }
    }
}
