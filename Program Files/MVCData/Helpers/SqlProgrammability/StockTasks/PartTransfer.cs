using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.StockTasks
{
    public class PartTransfer
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public PartTransfer(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPartTransferViewDetails();
            this.GetPendingPartTransferOrders();

            this.PartTransferSaveRelative();
            this.PartTransferPostSaveValidate();

            this.PartTransferDetailPrint();
        }



        private void GetPartTransferViewDetails()
        {
            string queryString;
            SqlProgrammability.StockTasks.Inventories inventories = new StockTasks.Inventories(this.totalBikePortalsEntities);

            queryString = " @StockTransferID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";


            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @LocationID varchar(35)      DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";
            queryString = queryString + "       SELECT      @EntryDate = EntryDate, @LocationID = LocationID FROM StockTransfers WHERE StockTransferID = @StockTransferID " + "\r\n";
            queryString = queryString + "       IF          @EntryDate IS NULL          SET @EntryDate = CONVERT(Datetime, '31/12/2000', 103)" + "\r\n";
            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID as varchar)  FROM Warehouses WHERE LocationID = @LocationID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID as varchar)  FROM StockTransferDetails WHERE StockTransferID = @StockTransferID FOR XML PATH('')) ,1,1,'') " + "\r\n";

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";

            queryString = queryString + "       SELECT      StockTransferDetails.StockTransferDetailID, StockTransferDetails.StockTransferID, StockTransferDetails.TransferOrderDetailID, StockTransferDetails.SupplierID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, " + "\r\n";
            queryString = queryString + "                   ROUND(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) + StockTransferDetails.Quantity, 0) AS QuantityAvailable, StockTransferDetails.Quantity, StockTransferDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        StockTransferDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON StockTransferDetails.StockTransferID = @StockTransferID AND StockTransferDetails.CommodityID = Commodities.CommodityID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON StockTransferDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN" + "\r\n";
            queryString = queryString + "                  (SELECT WarehouseID, CommodityID, SUM(QuantityEndREC) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON StockTransferDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND StockTransferDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPartTransferViewDetails", queryString);
        }

        private void GetPendingPartTransferOrders()
        {
            string queryString;
            SqlProgrammability.StockTasks.Inventories inventories = new StockTasks.Inventories(this.totalBikePortalsEntities);

            queryString = " @LocationID Int, @TransferOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";
            queryString = queryString + "       DECLARE     @CommoditiesAvailable TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL, QuantityAvailable decimal(18, 2) NOT NULL)" + "\r\n";

            queryString = queryString + "       DECLARE     @TransferOrderDetails TABLE (TransferOrderDetailID int NOT NULL, CommodityID int NOT NULL, WarehouseID int NOT NULL, Quantity decimal(18, 2) NOT NULL, QuantityTransfer decimal(18, 2) NOT NULL, Remarks nvarchar(100) NULL) " + "\r\n";
            queryString = queryString + "       INSERT INTO @TransferOrderDetails (TransferOrderDetailID, CommodityID, WarehouseID, Quantity, QuantityTransfer, Remarks) SELECT TransferOrderDetailID, CommodityID, WarehouseID, Quantity, QuantityTransfer, Remarks FROM TransferOrderDetails WHERE TransferOrderID = @TransferOrderID AND (CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Parts + " OR CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Consumables + ") AND ROUND(Quantity - QuantityTransfer, " + GlobalEnums.rndQuantity + ") > 0  " + "\r\n";


            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT DISTINCT ',' + CAST(WarehouseID as varchar) FROM @TransferOrderDetails FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT DISTINCT ',' + CAST(CommodityID as varchar) FROM @TransferOrderDetails FOR XML PATH('')) ,1,1,'') " + "\r\n";


            queryString = queryString + "       IF NOT @CommodityIDList IS NULL " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET             @EntryDate = GETDATE() " + "\r\n"; //GET INVENTORY UP TO DATE
            queryString = queryString + "               " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";
            queryString = queryString + "               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
            queryString = queryString + "               SELECT          WarehouseID, CommodityID, SUM(QuantityEndREC) AS QuantityAvailable " + "\r\n";
            queryString = queryString + "               FROM            @WarehouseJournalTable " + "\r\n";
            queryString = queryString + "               GROUP BY        WarehouseID, CommodityID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       SELECT      TransferOrderDetails.TransferOrderDetailID, NULL AS SupplierID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, " + "\r\n";
            queryString = queryString + "                   ROUND(TransferOrderDetails.Quantity - TransferOrderDetails.QuantityTransfer, " + GlobalEnums.rndQuantity + ") AS QuantityOrderPending, ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS QuantityAvailable, TransferOrderDetails.Remarks, CAST(IIF(CommoditiesAvailable.QuantityAvailable > 0, 1, 0) AS bit) AS IsSelected " + "\r\n";
            queryString = queryString + "       FROM        @TransferOrderDetails TransferOrderDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON TransferOrderDetails.WarehouseID = Warehouses.WarehouseID AND Warehouses.LocationID = @LocationID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON TransferOrderDetails.CommodityID = Commodities.CommodityID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   @CommoditiesAvailable CommoditiesAvailable ON TransferOrderDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND TransferOrderDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPendingPartTransferOrders", queryString);
        }

        private void PartTransferSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       EXEC        StockTransferUpdateTransferOrder @EntityID, @SaveRelativeOption " + "\r\n";

            queryString = queryString + "       SET         @SaveRelativeOption = -@SaveRelativeOption" + "\r\n";
            queryString = queryString + "       EXEC        UpdateWarehouseBalance @SaveRelativeOption, 0, 0, @EntityID, 0 ";

            this.totalBikePortalsEntities.CreateStoredProcedure("PartTransferSaveRelative", queryString);
        }

        private void PartTransferPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Transfer Order Date: ' + CAST(TransferOrders.EntryDate AS nvarchar) FROM StockTransfers INNER JOIN TransferOrders ON StockTransfers.StockTransferID = @EntityID AND StockTransfers.TransferOrderID = TransferOrders.TransferOrderID AND StockTransfers.EntryDate < TransferOrders.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Xuất kho vượt số lượng lệnh điều hàng: ' + Commodities.Code + '-' + Commodities.Name + ': ' + CAST(ROUND(TransferOrderDetails.Quantity - TransferOrderDetails.QuantityTransfer, 0) AS nvarchar) FROM TransferOrderDetails INNER JOIN Commodities ON TransferOrderDetails.CommodityID = Commodities.CommodityID WHERE (ROUND(TransferOrderDetails.Quantity - TransferOrderDetails.QuantityTransfer, 0) < 0) ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("PartTransferPostSaveValidate", queryArray);
        }




        private void PartTransferDetailPrint()
        {
            string queryString = " @SwitchSQLID int,  @EntityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalSwitchSQLID int,   @LocalEntityID int        SET @LocalSwitchSQLID = @SwitchSQLID        SET @LocalEntityID = @EntityID " + "\r\n";

            queryString = queryString + "       IF (@LocalSwitchSQLID = 1) " + "\r\n";

            queryString = queryString + "               SELECT          StockTransferDetails.StockTransferDetailID AS EntityID, Commodities.Code, Commodities.Name, StockTransferDetails.Quantity, WarehouseBalancePrice.UnitPrice, StockTransferDetails.Quantity * WarehouseBalancePrice.UnitPrice AS Amount " + "\r\n";
            queryString = queryString + "               FROM            StockTransferDetails " + "\r\n";
            queryString = queryString + "                               INNER JOIN Commodities ON StockTransferDetails.StockTransferID = @LocalEntityID AND StockTransferDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                               LEFT OUTER JOIN WarehouseBalancePrice ON dbo.EOMONTHTIME(StockTransferDetails.EntryDate, 9999) = WarehouseBalancePrice.EntryDate AND StockTransferDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               SELECT          GoodsReceiptDetails.GoodsReceiptDetailID AS EntityID, Commodities.Code, Commodities.Name, GoodsReceiptDetails.Quantity, WarehouseBalancePrice.UnitPrice, GoodsReceiptDetails.Quantity * WarehouseBalancePrice.UnitPrice AS Amount " + "\r\n";
            queryString = queryString + "               FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                               INNER JOIN Commodities ON GoodsReceiptDetails.GoodsReceiptID = @LocalEntityID AND GoodsReceiptDetails.CommodityTypeID <> " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                               LEFT OUTER JOIN WarehouseBalancePrice ON dbo.EOMONTHTIME(GoodsReceiptDetails.EntryDate, 9999) = WarehouseBalancePrice.EntryDate AND GoodsReceiptDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("PartTransferDetailPrint", queryString);
        }


    }
}
