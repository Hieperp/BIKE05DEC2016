using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;

using MVCBase.Enums;

using MVCModel.Models;

using MVCCore.Services.StockTasks;

using MVCDTO.StockTasks;

using MVCClient.ViewModels.StockTasks;
using MVCClient.Builders.StockTasks;
using MVCClient.ViewModels.Helpers;


namespace MVCClient.Controllers.StockTasks
{
    public class GoodsReceiptsController : GenericViewDetailController<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, GoodsReceiptDTO, GoodsReceiptPrimitiveDTO, GoodsReceiptDetailDTO, GoodsReceiptViewModel>
    {
        private readonly IGoodsReceiptService goodsReceiptService;
        IGoodsReceiptViewModelSelectListBuilder goodsReceiptViewModelSelectListBuilder;

        public GoodsReceiptsController(IGoodsReceiptService goodsReceiptService, IGoodsReceiptViewModelSelectListBuilder goodsReceiptViewModelSelectListBuilder)
            : base(goodsReceiptService, goodsReceiptViewModelSelectListBuilder, true)
        {
            this.goodsReceiptService = goodsReceiptService;
            this.goodsReceiptViewModelSelectListBuilder = goodsReceiptViewModelSelectListBuilder;
        }


        protected override ICollection<GoodsReceiptViewDetail> GetEntityViewDetails(GoodsReceiptViewModel goodsReceiptViewModel)
        {
            ICollection<GoodsReceiptViewDetail> goodsReceiptViewDetails = this.goodsReceiptService.GetGoodsReceiptViewDetails(goodsReceiptViewModel.GoodsReceiptID, goodsReceiptViewModel.GoodsReceiptTypeID, goodsReceiptViewModel.VoucherID, false);

            return goodsReceiptViewDetails;
        }

        protected override ViewModels.Helpers.PrintViewModel InitPrintViewModel(int? id)
        {
            PrintViewModel printViewModel = base.InitPrintViewModel(id);

            GoodsReceipt entity = base.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Readable);

            if (entity != null && entity.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice)
            {
                foreach (GoodsReceiptDetail goodsReceiptDetail in entity.GoodsReceiptDetails)
                {
                    if (goodsReceiptDetail.CommodityTypeID == (int)GlobalEnums.CommodityTypeID.Vehicles) printViewModel.PrintOptionID = printViewModel.PrintOptionID == 0 || printViewModel.PrintOptionID == 1 ? 1 : 3;
                    if (goodsReceiptDetail.CommodityTypeID != (int)GlobalEnums.CommodityTypeID.Vehicles) printViewModel.PrintOptionID = printViewModel.PrintOptionID == 0 || printViewModel.PrintOptionID == 2 ? 2 : 3;
                }
            }

            return printViewModel;
        }
    }
}

