using System.Collections.Generic;

using MVCModel.Models;


namespace MVCCore.Repositories.StockTasks
{
    public interface IGoodsReceiptRepository : IGenericWithDetailRepository<GoodsReceipt, GoodsReceiptDetail>
    {
        ICollection<GoodsReceiptGetPurchaseInvoice> GetPurchaseInvoices(int locationID, int? goodsReceiptID, string purchaseInvoiceReference);

        ICollection<GoodsReceiptGetStockTransfer> GetStockTransfers(int locationID, int? goodsReceiptID, string stockTransferReference);

        ICollection<AdditionalGoodsReceiptVoucherText> GetAdditionalGoodsReceiptVoucherText(int goodsReceiptTypeID, int voucherID);
    }

    public interface IGoodsReceiptAPIRepository : IGenericAPIRepository
    {
    }

}
