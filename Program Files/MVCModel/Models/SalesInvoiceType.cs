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
    
    public partial class SalesInvoiceType
    {
        public SalesInvoiceType()
        {
            this.SalesInvoices = new HashSet<SalesInvoice>();
        }
    
        public int SalesInvoiceTypeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int SerialID { get; set; }
    
        public virtual ICollection<SalesInvoice> SalesInvoices { get; set; }
    }
}