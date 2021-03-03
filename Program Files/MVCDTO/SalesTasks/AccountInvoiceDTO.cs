using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCBase.Enums;
using MVCDTO.Helpers;

namespace MVCDTO.SalesTasks
{
    public class AccountInvoicePrimitiveDTO : DiscountVATAmountDTO<AccountInvoiceDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.AccountInvoice; } }

        public int GetID() { return this.AccountInvoiceID; }
        public void SetID(int id) { this.AccountInvoiceID = id; }

        public int AccountInvoiceID { get; set; }

        public int CustomerID { get; set; }
        [Display(Name = "Khách hàng")]
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string CustomerName { get; set; }
        [Display(Name = "Ngày sinh")]
        public Nullable<System.DateTime> CustomerBirthday { get; set; }
        [Display(Name = "Mã số thuế")]
        public string CustomerVATCode { get; set; }
        [Display(Name = "Điện thoại")]
        public string CustomerTelephone { get; set; }
        [Display(Name = "Địa chỉ")]
        public string CustomerAddressNo { get; set; }
        [Display(Name = "Khu vực")]
        public string CustomerEntireTerritoryEntireName { get; set; }

        [Display(Name = "Số hóa đơn")]
        [Required(ErrorMessage = "Vui lòng nhập Số hóa đơn")]
        public string VATInvoiceNo { get; set; }
        [Display(Name = "Số seri")]
        [Required(ErrorMessage = "Vui lòng nhập Số seri")]
        public string VATInvoiceSeries { get; set; }
        [Display(Name = "Ngày hóa đơn")]
        [Required(ErrorMessage = "Vui lòng Ngày hóa đơn")]
        public Nullable<System.DateTime> VATInvoiceDate { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public int PaymentTermID { get; set; }


        #region
        public string ApiSerialString { get; set; }
        public int? ApiSerialID { get; set; }
        public string ApiMessage { get; set; }
        #endregion

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; });
        }
    }

    public class AccountInvoiceDTO : AccountInvoicePrimitiveDTO, IBaseDetailEntity<AccountInvoiceDetailDTO>
    {
        public AccountInvoiceDTO()
        {
            this.AccountInvoiceViewDetails = new List<AccountInvoiceDetailDTO>();
        }

        public List<AccountInvoiceDetailDTO> AccountInvoiceViewDetails { get; set; }
        public List<AccountInvoiceDetailDTO> ViewDetails { get { return this.AccountInvoiceViewDetails; } set { this.AccountInvoiceViewDetails = value; } }

        public ICollection<AccountInvoiceDetailDTO> GetDetails() { return this.AccountInvoiceViewDetails; }

        protected override IEnumerable<AccountInvoiceDetailDTO> DtoDetails() { return this.AccountInvoiceViewDetails; }
    }




    #region HELPER

    //https://json2csharp.com/xml-to-csharp


    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(Invoices));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Invoices)serializer.Deserialize(reader);
    // }

    public class Invoices
    {
        public Invoices()
        {
            this.Inv = new Inv();
        }

        public Inv Inv { get; set; }
        //public string Xsi { get; set; } = "@#@";
        //public string Xsd { get; set; } = "@#@";
        //public string Text { get; set; } = "@#@";
    }

    public class Inv
    {
        public Inv()
        {
            this.Invoice = new Invoice();
        }

        public string key { get; set; }
        public Invoice Invoice { get; set; }
    }

    public class Invoice
    {
        public Invoice()
        {
            this.Products = new List<Product>();
        }

        public string CusCode { get; set; } = "@#@";
        public string CusName { get; set; } = "@#@";
        public string CusAddress { get; set; } = "@#@";
        public string CusPhone { get; set; } = "@#@";
        public string CusTaxCode { get; set; } = "@#@";
        public string PaymentMethod { get; set; } = "@#@";
        public string KindOfService { get; set; } = "@#@";
        public string Total { get; set; } = "@#@";
        public string DiscountRate { get; set; } = "@#@";
        public string DiscountAmount { get; set; } = "@#@";
        public string VATRate { get; set; } = "@#@";
        public string VATAmount { get; set; } = "@#@";
        public string Amount { get; set; } = "@#@";
        public string AmountInWords { get; set; } = "@#@";
        public string ConvertedAmount { get; set; } = "@#@";
        public string Extra { get; set; } = "@#@";
        public string Extra1 { get; set; } = "@#@";
        public string Extra2 { get; set; } = "@#@";
        public string ArisingDate { get; set; } = "@#@";
        public string PaymentStatus { get; set; } = "@#@";
        public string ResourceCode { get; set; } = "@#@";
        public string GrossValue { get; set; } = "@#@";
        public string GrossValue0 { get; set; } = "@#@";
        public string VatAmount0 { get; set; } = "@#@";
        public string GrossValue5 { get; set; } = "@#@";
        public string VatAmount5 { get; set; } = "@#@";
        public string GrossValue10 { get; set; } = "@#@";
        public string VatAmount10 { get; set; } = "@#@";

        public List<Product> Products { get; set; }
    }

    public class Product
    {
        public string ProdName { get; set; } = "@#@";
        public string ProdUnit { get; set; } = "@#@";
        public string ProdQuantity { get; set; } = "@#@";
        public string ProdPrice { get; set; } = "@#@";
        public string Amount { get; set; } = "@#@";
        public string Remark { get; set; } = "@#@";
        public string Total { get; set; } = "@#@";
        public string VATRate { get; set; } = "@#@";
        public string VATAmount { get; set; } = "@#@";
        public string Extra1 { get; set; } = "@#@";
        public string Extra2 { get; set; } = "@#@";
        public string Discount { get; set; } = "@#@";
        public string DiscountAmount { get; set; } = "@#@";
        public string IsSum { get; set; } = "@#@";
    }

    #endregion
}
