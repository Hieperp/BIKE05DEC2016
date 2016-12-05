using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.StockTasks
{
    public class VehicleTransfer
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public VehicleTransfer(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetStockTransferIndexes();

            this.GetVehicleTransferViewDetails();
            this.GetPendingVehicleTransferOrders();

            this.StockTransferUpdateTransferOrder();

            this.VehicleTransferSaveRelative();
            this.VehicleTransferPostSaveValidate();

            this.StockTransferEditable();

            this.StockTransferInitReference();

            this.StockTransferPrint();
            this.VehicleTransferDetailPrint();
        }


        private void GetStockTransferIndexes()
        {
            this.GetStockTransferIndexesBUILDSQL("GetVehicleTransferIndexes", GlobalEnums.StockTransferTypeID.VehicleTransfer, GlobalEnums.NmvnTaskID.VehicleTransfer);
            this.GetStockTransferIndexesBUILDSQL("GetPartTransferIndexes", GlobalEnums.StockTransferTypeID.PartTransfer, GlobalEnums.NmvnTaskID.PartTransfer);
        }

        private void GetStockTransferIndexesBUILDSQL(string storedProcedureName, GlobalEnums.StockTransferTypeID stockTransferTypeID, GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Locations.Code AS LocationCode, StockTransfers.StockTransferID, CAST(StockTransfers.EntryDate AS DATE) AS EntryDate, StockTransfers.Reference, Warehouses.Code AS WarehouseCode, StockTransfers.TotalQuantity, StockTransfers.Description, TransferOrders.Reference AS TransferOrderReference, TransferOrders.EntryDate AS TransferOrderEntryDate, TransferOrders.RequestedDate AS TransferOrderRequestedDate " + "\r\n";
            queryString = queryString + "       FROM        StockTransfers INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON StockTransfers.StockTransferTypeID = " + (int)stockTransferTypeID + " AND StockTransfers.EntryDate >= @FromDate AND StockTransfers.EntryDate <= @ToDate AND StockTransfers.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)nmvnTaskID + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = StockTransfers.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Warehouses ON StockTransfers.WarehouseID = Warehouses.WarehouseID LEFT JOIN " + "\r\n";
            queryString = queryString + "                   TransferOrders ON StockTransfers.TransferOrderID = TransferOrders.TransferOrderID " + "\r\n";
            
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure(storedProcedureName, queryString);
        }


        private void GetVehicleTransferViewDetails()
        {
            string queryString;

            queryString = " @StockTransferID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      StockTransferDetails.StockTransferDetailID, StockTransferDetails.StockTransferID, StockTransferDetails.TransferOrderDetailID, StockTransferDetails.GoodsReceiptDetailID, StockTransferDetails.SupplierID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue + StockTransferDetails.Quantity, 0) AS QuantityAvailable, StockTransferDetails.Quantity, StockTransferDetails.Remarks" + "\r\n";
            queryString = queryString + "       FROM        StockTransferDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON StockTransferDetails.StockTransferID = @StockTransferID AND StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON GoodsReceiptDetails.CommodityID = Commodities.CommodityID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetVehicleTransferViewDetails", queryString);
        }


        private void GetPendingVehicleTransferOrders()
        {
            string queryString;

            queryString = " @LocationID Int, @TransferOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      TransferOrderDetails.TransferOrderDetailID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.SupplierID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   ROUND(TransferOrderDetails.Quantity - TransferOrderDetails.QuantityTransfer, " + GlobalEnums.rndQuantity + ") AS QuantityOrderPending, ROUND(ISNULL(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue, 0), " + GlobalEnums.rndQuantity + ") AS QuantityAvailable, TransferOrderDetails.Remarks, CAST(IIF(TransferOrderDetails.Remarks = GoodsReceiptDetails.ChassisCode + '#' + GoodsReceiptDetails.EngineCode AND ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue, " + GlobalEnums.rndQuantity + ") > 0, 1, 0) AS bit) AS IsSelected" + "\r\n";
            queryString = queryString + "       FROM        TransferOrderDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON TransferOrderDetails.TransferOrderID = @TransferOrderID AND TransferOrderDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND ROUND(TransferOrderDetails.Quantity - TransferOrderDetails.QuantityTransfer, " + GlobalEnums.rndQuantity + ") > 0 AND TransferOrderDetails.WarehouseID = Warehouses.WarehouseID AND Warehouses.LocationID = @LocationID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON TransferOrderDetails.CommodityID = Commodities.CommodityID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue, " + GlobalEnums.rndQuantity + ") > 0 AND TransferOrderDetails.WarehouseID = GoodsReceiptDetails.WarehouseID AND TransferOrderDetails.CommodityID = GoodsReceiptDetails.CommodityID " + "\r\n";
            
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPendingVehicleTransferOrders", queryString);
        }

        private void StockTransferUpdateTransferOrder()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       UPDATE      TransferOrderDetails" + "\r\n";
            queryString = queryString + "       SET         QuantityTransfer = ROUND(TransferOrderDetails.QuantityTransfer + StockTransferDetails.Quantity * @SaveRelativeOption, 0)" + "\r\n";
            queryString = queryString + "       FROM       (SELECT TransferOrderDetailID, SUM(Quantity) AS Quantity FROM StockTransferDetails WHERE StockTransferID = @EntityID AND TransferOrderDetailID IS NOT NULL GROUP BY TransferOrderDetailID) StockTransferDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   TransferOrderDetails ON StockTransferDetails.TransferOrderDetailID = TransferOrderDetails.TransferOrderDetailID" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("StockTransferUpdateTransferOrder", queryString);
        }

        private void VehicleTransferSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       EXEC        StockTransferUpdateTransferOrder @EntityID, @SaveRelativeOption " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsReceiptDetails" + "\r\n";
            queryString = queryString + "       SET         QuantityIssue = ROUND(GoodsReceiptDetails.QuantityIssue + StockTransferDetails.Quantity * @SaveRelativeOption, 0)" + "\r\n";
            queryString = queryString + "       FROM       (SELECT GoodsReceiptDetailID, SUM(Quantity) AS Quantity FROM StockTransferDetails WHERE StockTransferID = @EntityID GROUP BY GoodsReceiptDetailID) StockTransferDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("VehicleTransferSaveRelative", queryString);

        }

        private void VehicleTransferPostSaveValidate()
        {
            string[] queryArray = new string[4];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Warehouse Date: ' + CAST(GoodsReceiptDetails.EntryDate AS nvarchar) FROM StockTransferDetails INNER JOIN GoodsReceiptDetails ON StockTransferDetails.StockTransferID = @EntityID AND StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID AND StockTransferDetails.EntryDate < GoodsReceiptDetails.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = 'Transfer Order Date: ' + CAST(TransferOrders.EntryDate AS nvarchar) FROM StockTransfers INNER JOIN TransferOrders ON StockTransfers.StockTransferID = @EntityID AND StockTransfers.TransferOrderID = TransferOrders.TransferOrderID AND StockTransfers.EntryDate < TransferOrders.EntryDate ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = 'Over Quantity: ' + CAST(ROUND(Quantity - QuantityIssue, 0) AS nvarchar) FROM GoodsReceiptDetails WHERE (ROUND(Quantity - QuantityIssue, 0) < 0) ";
            queryArray[3] = " SELECT TOP 1 @FoundEntity = N'Xuất kho vượt số lượng lệnh điều hàng: ' + Commodities.Code + '-' + Commodities.Name + ': ' + CAST(ROUND(TransferOrderDetails.Quantity - TransferOrderDetails.QuantityTransfer, 0) AS nvarchar) FROM TransferOrderDetails INNER JOIN Commodities ON TransferOrderDetails.CommodityID = Commodities.CommodityID WHERE (ROUND(TransferOrderDetails.Quantity - TransferOrderDetails.QuantityTransfer, 0) < 0) ";
            
            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("VehicleTransferPostSaveValidate", queryArray);
        }


        private void StockTransferEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = VoucherID FROM GoodsReceipts WHERE GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND VoucherID = @EntityID ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("StockTransferEditable", queryArray);
        }


        private void StockTransferInitReference()
        {
            StockTransferInitReference simpleInitReference = new StockTransferInitReference("StockTransfers", "StockTransferID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.StockTransfer));
            this.totalBikePortalsEntities.CreateTrigger("StockTransferInitReference", simpleInitReference.CreateQuery());
        }














        private void StockTransferPrint()
        {
            string queryString = " @SwitchSQLID int,  @EntityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalSwitchSQLID int,   @LocalEntityID int        SET @LocalSwitchSQLID = @SwitchSQLID        SET @LocalEntityID = @EntityID " + "\r\n";

            queryString = queryString + "       IF (@LocalSwitchSQLID = 1) " + "\r\n";

            queryString = queryString + "               SELECT          StockTransfers.StockTransferID AS EntityID, StockTransfers.EntryDate, StockTransfers.Reference, WarehouseLocations.OfficialName AS WarehouseLocationOfficialName, WarehouseLocations.Address AS WarehouseLocationAddress, " + "\r\n";
            queryString = queryString + "                               CompanyLocations.Name AS CompanyName, CompanyLocations.OfficialName AS CompanyOfficialName, CompanyLocations.Address AS CompanyAddress, " + "\r\n";
            queryString = queryString + "                               Locations.Name AS LocationName, Locations.OfficialName AS LocationOfficialName, Locations.Address AS LocationAddress, TransferOrders.Reference AS VoucherReference, TransferOrders.EntryDate AS VoucherEntryDate " + "\r\n";
            queryString = queryString + "               FROM            StockTransfers " + "\r\n";

            queryString = queryString + "                               INNER JOIN Locations CompanyLocations ON StockTransfers.StockTransferID = @LocalEntityID AND CompanyLocations.LocationID = 9 " + "\r\n"; //LocationID = 9: CTY

            queryString = queryString + "                               INNER JOIN Locations ON StockTransfers.LocationID = Locations.LocationID " + "\r\n";
            queryString = queryString + "                               INNER JOIN Warehouses ON StockTransfers.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                               INNER JOIN Locations AS WarehouseLocations ON Warehouses.LocationID = WarehouseLocations.LocationID " + "\r\n";
            queryString = queryString + "                               LEFT OUTER JOIN TransferOrders ON StockTransfers.TransferOrderID = TransferOrders.TransferOrderID " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               SELECT          GoodsReceipts.GoodsReceiptID AS EntityID, GoodsReceipts.EntryDate, GoodsReceipts.Reference, WarehouseLocations.OfficialName AS WarehouseLocationOfficialName, WarehouseLocations.Address AS WarehouseLocationAddress, " + "\r\n";
            queryString = queryString + "                               CompanyLocations.Name AS CompanyName, CompanyLocations.OfficialName AS CompanyOfficialName, CompanyLocations.Address AS CompanyAddress, " + "\r\n";
            queryString = queryString + "                               CompanyLocations.Name AS LocationName, CompanyLocations.OfficialName AS LocationOfficialName, CompanyLocations.Address AS LocationAddress, PurchaseInvoices.VATInvoiceNo AS VoucherReference, PurchaseInvoices.VATInvoiceDate AS VoucherEntryDate " + "\r\n";
            queryString = queryString + "               FROM            GoodsReceipts " + "\r\n";
            queryString = queryString + "                               INNER JOIN Locations AS CompanyLocations ON GoodsReceipts.GoodsReceiptID = @LocalEntityID AND CompanyLocations.LocationID = 9 " + "\r\n"; //LocationID = 9: CTY
            queryString = queryString + "                               INNER JOIN Locations AS WarehouseLocations ON GoodsReceipts.LocationID = WarehouseLocations.LocationID " + "\r\n";
            queryString = queryString + "                               INNER JOIN PurchaseInvoices ON GoodsReceipts.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND GoodsReceipts.VoucherID = PurchaseInvoices.PurchaseInvoiceID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("StockTransferPrint", queryString);
        }


        private void VehicleTransferDetailPrint()
        {
            string queryString = " @SwitchSQLID int,  @EntityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalSwitchSQLID int,   @LocalEntityID int        SET @LocalSwitchSQLID = @SwitchSQLID        SET @LocalEntityID = @EntityID " + "\r\n";

            queryString = queryString + "       IF (@LocalSwitchSQLID = 1) " + "\r\n";

            queryString = queryString + "               SELECT          StockTransferDetails.StockTransferDetailID AS EntityID, Commodities.Code, Commodities.Name, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, StockTransferDetails.Quantity, GoodsReceiptDetails.UnitPrice, StockTransferDetails.Quantity * GoodsReceiptDetails.UnitPrice AS Amount " + "\r\n";
            queryString = queryString + "               FROM            StockTransferDetails " + "\r\n";
            queryString = queryString + "                               INNER JOIN GoodsReceiptDetails ON StockTransferDetails.StockTransferID = @LocalEntityID AND StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                               INNER JOIN Commodities ON StockTransferDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               SELECT          GoodsReceiptDetails.GoodsReceiptDetailID AS EntityID, Commodities.Code, Commodities.Name, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.Quantity, GoodsReceiptDetails.UnitPrice, GoodsReceiptDetails.Quantity * GoodsReceiptDetails.UnitPrice AS Amount " + "\r\n";
            queryString = queryString + "               FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                               INNER JOIN Commodities ON GoodsReceiptDetails.GoodsReceiptID = @LocalEntityID AND GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("VehicleTransferDetailPrint", queryString);
        }

    }
}
