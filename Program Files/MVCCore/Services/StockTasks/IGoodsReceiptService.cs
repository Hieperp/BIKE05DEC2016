using System.Collections.Generic;

using MVCModel.Models;
using MVCDTO.StockTasks;
using MVCCore.Services.Helpers;

namespace MVCCore.Services.StockTasks
{
    public interface IGoodsReceiptService : IGenericWithViewDetailService<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, GoodsReceiptDTO, GoodsReceiptPrimitiveDTO, GoodsReceiptDetailDTO>
    {
        ICollection<GoodsReceiptViewDetail> GetGoodsReceiptViewDetails(int goodsReceiptID, int goodsReceiptVoucherTypeID, int goodsReceiptVoucherID, bool isReadOnly);        
    }

    public interface IGoodsReceiptHelperService : IHelperService<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptDTO, GoodsReceiptDetailDTO>
    {
    }
}
