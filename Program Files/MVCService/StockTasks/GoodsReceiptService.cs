using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCBase.Enums;
using MVCModel.Models;
using MVCDTO.StockTasks;

using MVCCore.Repositories.CommonTasks;
using MVCCore.Repositories.StockTasks;
using MVCCore.Services.Helpers;
using MVCCore.Services.StockTasks;

using MVCService.Helpers;


namespace MVCService.StockTasks
{
    public class GoodsReceiptService : GenericWithViewDetailService<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, GoodsReceiptDTO, GoodsReceiptPrimitiveDTO, GoodsReceiptDetailDTO>, IGoodsReceiptService
    {
        private DateTime? checkedDate; //For check over stock
        private string warehouseIDList = "";
        private string commodityIDList = "";

        private readonly IInventoryRepository inventoryRepository;
        private readonly IGoodsReceiptHelperService goodsReceiptHelperService;


        public GoodsReceiptService(IGoodsReceiptRepository goodsReceiptRepository, IInventoryRepository inventoryRepository, IGoodsReceiptHelperService goodsReceiptHelperService)
            : base(goodsReceiptRepository, "GoodsReceiptPostSaveValidate", "GoodsReceiptSaveRelative", null, "GetGoodsReceiptViewDetails")
        {
            this.inventoryRepository = inventoryRepository;
            this.goodsReceiptHelperService = goodsReceiptHelperService;
        }

        public override ICollection<GoodsReceiptViewDetail> GetViewDetails(int goodsReceiptID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsReceiptID", -1), new ObjectParameter("GoodsReceiptTypeID", -1), new ObjectParameter("VoucherID", -1), new ObjectParameter("IsReadOnly", false) };
            return this.GetViewDetails(parameters); //Always return empty collection. use GetGoodsReceiptViewDetails instead
        }

        public ICollection<GoodsReceiptViewDetail> GetGoodsReceiptViewDetails(int goodsReceiptID, int goodsReceiptTypeID, int voucherID, bool isReadOnly)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsReceiptID", goodsReceiptID), new ObjectParameter("GoodsReceiptTypeID", goodsReceiptTypeID), new ObjectParameter("VoucherID", voucherID), new ObjectParameter("IsReadOnly", isReadOnly) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(GoodsReceiptDTO goodsReceiptDTO)
        {
            goodsReceiptDTO.GoodsReceiptViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(goodsReceiptDTO);
        }

        protected override void UpdateDetail(GoodsReceiptDTO dto, GoodsReceipt entity)
        {
            this.goodsReceiptHelperService.GetWCParameters(dto, null, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UpdateDetail(dto, entity);
        }

        protected override void UndoDetail(GoodsReceiptDTO dto, GoodsReceipt entity, bool isDelete)
        {
            this.goodsReceiptHelperService.GetWCParameters(null, entity, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UndoDetail(dto, entity, isDelete);
        }

        protected override void PostSaveValidate(GoodsReceipt entity)
        {
            this.inventoryRepository.CheckOverStock(this.checkedDate, this.warehouseIDList, this.commodityIDList);
            base.PostSaveValidate(entity);
        }
    }


    public class GoodsReceiptHelperService : HelperService<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptDTO, GoodsReceiptDetailDTO>, IGoodsReceiptHelperService
    {
    }

}
