using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;

namespace MVCDTO.PurchaseTasks
{
    public class PurchaseInvoiceDetailDTO : PurchaseDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.PurchaseInvoiceDetailID; }

        public int PurchaseInvoiceDetailID { get; set; }
        public int PurchaseInvoiceID { get; set; }

        [Display(Name = "Ngày đặt mua")]
        public System.DateTime PurchaseOrderDate { get; set; }
        [Display(Name = "Số phiếu")]
        public string PurchaseOrderReference { get; set; }
        [Display(Name = "Số phiếu xác nhận")]
        public string ConfirmReference { get; set; }

        [Display(Name = "SL ĐH")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }
    }
}
