using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCBase.Enums;

namespace MVCDTO.PurchaseTasks
{
    public class PurchaseInvoicePrimitiveDTO : PurchasePrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PurchaseInvoice; } }

        public int GetID() { return this.PurchaseInvoiceID; }
        public void SetID(int id) { this.PurchaseInvoiceID = id; }

        public int PurchaseInvoiceID { get; set; }
        [Display(Name = "Số hóa đơn")]
        public string VATInvoiceNo { get; set; }
        [Display(Name = "Ngày hóa đơn")]
        public Nullable<System.DateTime> VATInvoiceDate { get; set; }

        public Nullable<int> PurchaseOrderID { get; set; }
        [Display(Name = "Số phiếu mua")]
        public string PurchaseOrderReference { get; set; }
        [Display(Name = "Ngày mua")]
        public Nullable<System.DateTime> PurchaseOrderEntryDate { get; set; }

        public string PurchaseOrderAttentionName { get; set; }
        public string PurchaseOrderDescription { get; set; }
        public string PurchaseOrderRemarks { get; set; }

        [Display(Name = "Thời hạn thanh toán")]
        public Nullable<System.DateTime> DueDate { get; set; }
    }

    public class PurchaseInvoiceDTO : PurchaseInvoicePrimitiveDTO, IBaseDetailEntity<PurchaseInvoiceDetailDTO>
    {
        public PurchaseInvoiceDTO()
        {
            this.PurchaseInvoiceViewDetails = new List<PurchaseInvoiceDetailDTO>();
        }


        public List<PurchaseInvoiceDetailDTO> PurchaseInvoiceViewDetails { get; set; }
        public List<PurchaseInvoiceDetailDTO> ViewDetails { get { return this.PurchaseInvoiceViewDetails; } set { this.PurchaseInvoiceViewDetails = value; } }

        public ICollection<PurchaseInvoiceDetailDTO> GetDetails() { return this.PurchaseInvoiceViewDetails; }

        protected override IEnumerable<PurchaseDetailDTO> DtoDetails() { return this.PurchaseInvoiceViewDetails; }
    }
}
