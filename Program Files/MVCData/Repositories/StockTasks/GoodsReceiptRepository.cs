using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

using MVCModel.Models;
using MVCCore.Repositories.StockTasks;
using MVCData.Helpers;

namespace MVCData.Repositories.StockTasks
{
    public class GoodsReceiptRepository : GenericWithDetailRepository<GoodsReceipt, GoodsReceiptDetail>, IGoodsReceiptRepository
    {
        public GoodsReceiptRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GoodsReceiptEditable")
        {
        }

        public ICollection<GoodsReceiptGetPurchaseInvoice> GetPurchaseInvoices(int locationID, int? goodsReceiptID, string purchaseInvoiceReference)
        {
            return this.TotalBikePortalsEntities.GoodsReceiptGetPurchaseInvoices(locationID, goodsReceiptID, purchaseInvoiceReference).ToList();
        }

        public ICollection<GoodsReceiptGetStockTransfer> GetStockTransfers(int locationID, int? goodsReceiptID, string stockTransferReference)
        {
            return this.TotalBikePortalsEntities.GoodsReceiptGetStockTransfers(locationID, goodsReceiptID, stockTransferReference).ToList();
        }

        public ICollection<AdditionalGoodsReceiptVoucherText> GetAdditionalGoodsReceiptVoucherText(int goodsReceiptTypeID, int voucherID)
        {
            return this.TotalBikePortalsEntities.GetAdditionalGoodsReceiptVoucherText(goodsReceiptTypeID, voucherID).ToList();
        }

    }

    public class GoodsReceiptAPIRepository : GenericAPIRepository, IGoodsReceiptAPIRepository
    {
        public GoodsReceiptAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetGoodsReceiptIndexes")
        {
        }
    }

}
