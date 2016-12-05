using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.CommonTasks
{
    class Commodity
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public Commodity(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.CommodityDeletable();
        }


        private void CommodityDeletable()
        {
            string[] queryArray = new string[10];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = CommodityID FROM PurchaseOrderDetails WHERE CommodityID = @EntityID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = CommodityID FROM PurchaseInvoiceDetails WHERE CommodityID = @EntityID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = CommodityID FROM GoodsReceiptDetails WHERE CommodityID = @EntityID ";
            queryArray[3] = " SELECT TOP 1 @FoundEntity = CommodityID FROM QuotationDetails WHERE CommodityID = @EntityID ";
            queryArray[4] = " SELECT TOP 1 @FoundEntity = CommodityID FROM SalesInvoiceDetails WHERE CommodityID = @EntityID ";
            queryArray[5] = " SELECT TOP 1 @FoundEntity = CommodityID FROM ServiceContracts WHERE CommodityID = @EntityID ";
            queryArray[6] = " SELECT TOP 1 @FoundEntity = CommodityID FROM TransferOrderDetails WHERE CommodityID = @EntityID ";
            queryArray[7] = " SELECT TOP 1 @FoundEntity = CommodityID FROM StockTransferDetails WHERE CommodityID = @EntityID ";
            queryArray[8] = " SELECT TOP 1 @FoundEntity = CommodityID FROM WarehouseBalanceDetail WHERE CommodityID = @EntityID ";
            queryArray[9] = " SELECT TOP 1 @FoundEntity = CommodityID FROM WarehouseBalancePrice WHERE CommodityID = @EntityID ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("CommodityDeletable", queryArray);
        }

    }
}
