using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.StockTasks
{
    public class TransferOrder
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public TransferOrder(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetTransferOrderIndexes();

            this.GetVehicleTransferOrderViewDetails();
            this.GetPartTransferOrderViewDetails();

            this.TransferOrderApproved();
            this.TransferOrderEditable();

            this.TransferOrderToggleApproved();
            this.TransferOrderVoid();

            this.TransferOrderInitReference();
        }


        private void GetTransferOrderIndexes()
        {
            this.GetTransferOrderIndexesBUILDSQL("GetVehicleTransferOrderIndexes", GlobalEnums.StockTransferTypeID.VehicleTransfer, GlobalEnums.NmvnTaskID.VehicleTransferOrder);
            this.GetTransferOrderIndexesBUILDSQL("GetPartTransferOrderIndexes", GlobalEnums.StockTransferTypeID.PartTransfer, GlobalEnums.NmvnTaskID.PartTransferOrder);
        }

        private void GetTransferOrderIndexesBUILDSQL(string storedProcedureName, GlobalEnums.StockTransferTypeID stockTransferTypeID, GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Locations.Code AS LocationCode, TransferOrders.TransferOrderID, CAST(TransferOrders.EntryDate AS DATE) AS EntryDate, TransferOrders.Reference, TransferOrders.RequestedDate, SourceLocations.Code AS SourceLocationCode, Warehouses.Code AS WarehouseCode, TransferOrders.TotalQuantity, TransferOrders.Description " + "\r\n";
            queryString = queryString + "       FROM        TransferOrders INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON TransferOrders.StockTransferTypeID = " + (int)stockTransferTypeID + " AND TransferOrders.EntryDate >= @FromDate AND TransferOrders.EntryDate <= @ToDate AND TransferOrders.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)nmvnTaskID + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = TransferOrders.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Locations SourceLocations ON TransferOrders.SourceLocationID = SourceLocations.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Warehouses ON TransferOrders.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure(storedProcedureName, queryString);
        }


        private void GetVehicleTransferOrderViewDetails()
        {
            string queryString;

            queryString = " @TransferOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @TransferOrderDetails TABLE (TransferOrderDetailID int NOT NULL, EntryDate datetime NOT NULL, TransferOrderID int NOT NULL, CommodityID int NOT NULL, CommodityTypeID int NOT NULL, WarehouseID int NOT NULL, Quantity decimal(18, 2) NOT NULL, Remarks nvarchar(100) NULL) " + "\r\n";
            queryString = queryString + "       INSERT INTO @TransferOrderDetails (TransferOrderDetailID, EntryDate, TransferOrderID, CommodityID, CommodityTypeID, WarehouseID, Quantity, Remarks) SELECT TransferOrderDetailID, EntryDate, TransferOrderID, CommodityID, CommodityTypeID, WarehouseID, Quantity, Remarks FROM TransferOrderDetails WHERE TransferOrderID = @TransferOrderID " + "\r\n";

            queryString = queryString + "       SELECT      TransferOrderDetails.TransferOrderDetailID, TransferOrderDetails.TransferOrderID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, " + "\r\n";
            queryString = queryString + "                   ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS QuantityAvailable, TransferOrderDetails.Quantity, TransferOrderDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        @TransferOrderDetails TransferOrderDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON TransferOrderDetails.WarehouseID = Warehouses.WarehouseID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON TransferOrderDetails.CommodityID = Commodities.CommodityID LEFT JOIN" + "\r\n";
            queryString = queryString + "                  (SELECT WarehouseID, CommodityID, SUM(Quantity - QuantityIssue) AS QuantityAvailable FROM GoodsReceiptDetails WHERE ROUND(Quantity - QuantityIssue, 0) > 0 AND CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND WarehouseID IN (SELECT DISTINCT WarehouseID FROM @TransferOrderDetails) AND CommodityID IN (SELECT DISTINCT CommodityID FROM @TransferOrderDetails) GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON TransferOrderDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND TransferOrderDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetVehicleTransferOrderViewDetails", queryString);
        }

        private void GetPartTransferOrderViewDetails()
        {
            string queryString;
            SqlProgrammability.StockTasks.Inventories inventories = new StockTasks.Inventories(this.totalBikePortalsEntities);

            queryString = " @TransferOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";
            queryString = queryString + "       DECLARE     @CommoditiesAvailable TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL, QuantityAvailable decimal(18, 2) NOT NULL)" + "\r\n";

            queryString = queryString + "       DECLARE     @TransferOrderDetails TABLE (TransferOrderDetailID int NOT NULL, EntryDate datetime NOT NULL, TransferOrderID int NOT NULL, CommodityID int NOT NULL, CommodityTypeID int NOT NULL, WarehouseID int NOT NULL, Quantity decimal(18, 2) NOT NULL, Remarks nvarchar(100) NULL) " + "\r\n";
            queryString = queryString + "       INSERT INTO @TransferOrderDetails (TransferOrderDetailID, EntryDate, TransferOrderID, CommodityID, CommodityTypeID, WarehouseID, Quantity, Remarks) SELECT TransferOrderDetailID, EntryDate, TransferOrderID, CommodityID, CommodityTypeID, WarehouseID, Quantity, Remarks FROM TransferOrderDetails WHERE TransferOrderID = @TransferOrderID " + "\r\n";


            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT DISTINCT ',' + CAST(WarehouseID as varchar) FROM @TransferOrderDetails FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT DISTINCT ',' + CAST(CommodityID as varchar) FROM @TransferOrderDetails WHERE CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Parts + ", " + (int)GlobalEnums.CommodityTypeID.Consumables + ") FOR XML PATH('')) ,1,1,'') " + "\r\n";


            queryString = queryString + "       IF NOT @CommodityIDList IS NULL " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET             @EntryDate = GETDATE() " + "\r\n"; //GET INVENTORY UP TO DATE
            queryString = queryString + "               " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";
            queryString = queryString + "               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
            queryString = queryString + "               SELECT          WarehouseID, CommodityID, QuantityEndREC AS QuantityAvailable " + "\r\n";
            queryString = queryString + "               FROM            @WarehouseJournalTable " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       SELECT      TransferOrderDetails.TransferOrderDetailID, TransferOrderDetails.TransferOrderID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, " + "\r\n";
            queryString = queryString + "                   ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS QuantityAvailable, TransferOrderDetails.Quantity, TransferOrderDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        @TransferOrderDetails TransferOrderDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON TransferOrderDetails.WarehouseID = Warehouses.WarehouseID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON TransferOrderDetails.CommodityID = Commodities.CommodityID LEFT JOIN" + "\r\n";
            queryString = queryString + "                  (SELECT WarehouseID, CommodityID, SUM(QuantityAvailable) AS QuantityAvailable FROM @CommoditiesAvailable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON TransferOrderDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND TransferOrderDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPartTransferOrderViewDetails", queryString);
        }

        private void TransferOrderApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = TransferOrderID FROM TransferOrders WHERE TransferOrderID = @EntityID AND Approved = 1";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("TransferOrderApproved", queryArray);
        }

        private void TransferOrderEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = TransferOrderID FROM StockTransfers WHERE TransferOrderID = @EntityID ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("TransferOrderEditable", queryArray);
        }


        private void TransferOrderToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      TransferOrders  SET Approved = @Approved, ApprovedDate = GetDate() WHERE TransferOrderID = @EntityID AND Approved = ~@Approved" + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("TransferOrderToggleApproved", queryString);
        }

        private void TransferOrderVoid()
        {
            string queryString = " @EntityID int, @InActive bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      TransferOrders  SET InActive = @InActive, InActiveDate = GetDate() WHERE TransferOrderID = @EntityID " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("TransferOrderVoid", queryString);
        }


        private void TransferOrderInitReference()
        {
            TransferOrderInitReference simpleInitReference = new TransferOrderInitReference("TransferOrders", "TransferOrderID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.TransferOrder));
            this.totalBikePortalsEntities.CreateTrigger("TransferOrderInitReference", simpleInitReference.CreateQuery());

        }
    }
}
