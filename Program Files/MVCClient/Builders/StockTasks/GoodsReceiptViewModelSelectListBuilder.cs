using System.Linq;

using MVCCore.Repositories.CommonTasks;
using MVCCore.Repositories.StockTasks;

using MVCModel.Models;
using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.StockTasks;

namespace MVCClient.Builders.StockTasks
{
    public class GoodsReceiptViewModelSelectListBuilder : IGoodsReceiptViewModelSelectListBuilder
    {
        private readonly IGoodsReceiptRepository goodsReceiptRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public GoodsReceiptViewModelSelectListBuilder(IGoodsReceiptRepository goodsReceiptRepository,
                                    IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.goodsReceiptRepository = goodsReceiptRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(GoodsReceiptViewModel goodsReceiptViewModel)
        {
            goodsReceiptViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), goodsReceiptViewModel.UserID);
            goodsReceiptViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), goodsReceiptViewModel.UserID);

            var additionalGoodsReceiptVoucherText = this.goodsReceiptRepository.GetAdditionalGoodsReceiptVoucherText(goodsReceiptViewModel.GoodsReceiptTypeID, goodsReceiptViewModel.VoucherID).SingleOrDefault();
            if (additionalGoodsReceiptVoucherText != null)
            {
                goodsReceiptViewModel.VoucherText1 = additionalGoodsReceiptVoucherText.VoucherText1;
                goodsReceiptViewModel.VoucherText2 = additionalGoodsReceiptVoucherText.VoucherText2;
                goodsReceiptViewModel.VoucherText3 = additionalGoodsReceiptVoucherText.VoucherText3;
                goodsReceiptViewModel.VoucherText4 = additionalGoodsReceiptVoucherText.VoucherText4;
                goodsReceiptViewModel.VoucherText5 = (additionalGoodsReceiptVoucherText.VATInvoiceNo != null ? "VAT: " + additionalGoodsReceiptVoucherText.VATInvoiceNo + " " : "") +  (additionalGoodsReceiptVoucherText.VATInvoiceDate != null? " [" + additionalGoodsReceiptVoucherText.VATInvoiceDate.Value.ToShortDateString() + "] " : "") + additionalGoodsReceiptVoucherText.Reference + " [" + additionalGoodsReceiptVoucherText.EntryDate.ToShortDateString() + "]";
            }
        }

    }
}