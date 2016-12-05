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

}
