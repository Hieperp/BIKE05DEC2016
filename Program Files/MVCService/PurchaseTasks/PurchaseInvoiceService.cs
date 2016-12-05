using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCModel.Models;
using MVCDTO.PurchaseTasks;
using MVCCore.Repositories.PurchaseTasks;
using MVCCore.Services.PurchaseTasks;


namespace MVCService.PurchaseTasks
{
    public class PurchaseInvoiceService : GenericWithViewDetailService<PurchaseInvoice, PurchaseInvoiceDetail, PurchaseInvoiceViewDetail, PurchaseInvoiceDTO, PurchaseInvoicePrimitiveDTO, PurchaseInvoiceDetailDTO>, IPurchaseInvoiceService
    {
        private readonly IPurchaseInvoiceRepository purchaseInvoiceRepository;

        public PurchaseInvoiceService(IPurchaseInvoiceRepository purchaseInvoiceRepository)
            : base(purchaseInvoiceRepository, "PurchaseInvoicePostSaveValidate", "PurchaseInvoiceSaveRelative", null, "GetPurchaseInvoiceViewDetails")
        {
            this.purchaseInvoiceRepository = purchaseInvoiceRepository;
        }

        public override ICollection<PurchaseInvoiceViewDetail> GetViewDetails(int purchaseInvoiceID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetPurchaseInvoiceViewDetails instead.", "Purchase Invoice Service");
        }

        public ICollection<PurchaseInvoiceViewDetail> GetPurchaseInvoiceViewDetails(int purchaseInvoiceID, int purchaseOrderID, int supplierID, bool isReadOnly)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PurchaseInvoiceID", purchaseInvoiceID), new ObjectParameter("PurchaseOrderID", purchaseOrderID), new ObjectParameter("SupplierID", supplierID), new ObjectParameter("IsReadOnly", isReadOnly) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(PurchaseInvoiceDTO purchaseInvoiceDTO)
        {
            purchaseInvoiceDTO.PurchaseInvoiceViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(purchaseInvoiceDTO);
        }

        protected override void AlterMe(PurchaseInvoiceDTO dto)
        {
            if (dto.VATInvoiceDate == null || dto.VATInvoiceDate.Value.Year != dto.EntryDate.Value.Year || dto.VATInvoiceDate.Value.Month != dto.EntryDate.Value.Month) throw new System.ArgumentException("Lỗi dữ liệu", "Ngày hóa đơn không hợp lệ. Ngày nhập hàng và ngày hóa đơn phải cùng trong tháng.");

            SqlParameter dueDateParameter = new SqlParameter("DueDate", SqlDbType.DateTime); dueDateParameter.Value = (dto.DueDate == null ? SqlDateTime.Null : (SqlDateTime)dto.DueDate);
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("PurchaseInvoiceID", dto.GetID()), new SqlParameter("VATInvoiceNo", dto.VATInvoiceNo), new SqlParameter("VATInvoiceDate", dto.VATInvoiceDate), dueDateParameter };

            if (this.purchaseInvoiceRepository.ExecuteStoreCommand("UPDATE PurchaseInvoices SET VATInvoiceNo = @VATInvoiceNo, VATInvoiceDate = @VATInvoiceDate, DueDate = @DueDate WHERE PurchaseInvoiceID = @PurchaseInvoiceID", parameters) != 1) throw new System.ArgumentException("Lỗi điều chỉnh dữ liệu", "Không thể điều chỉnh hóa đơn mua hàng.");
        }
    }
}
