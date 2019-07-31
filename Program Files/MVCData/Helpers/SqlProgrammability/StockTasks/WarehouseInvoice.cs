using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.StockTasks
{
    public class WarehouseInvoice
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public WarehouseInvoice(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetWarehouseInvoiceIndexes();

            this.GetWarehouseInvoiceViewDetails();
            this.GetPendingStockTransfers();

            this.WarehouseInvoiceSaveRelative();
            this.WarehouseInvoicePostSaveValidate();

            this.WarehouseInvoiceInitReference();

            this.WarehouseInvoiceSheet();
        }

        private void GetWarehouseInvoiceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      WarehouseInvoices.WarehouseInvoiceID, CAST(WarehouseInvoices.EntryDate AS DATE) AS EntryDate, WarehouseInvoices.Reference, WarehouseInvoices.VATInvoiceNo, Locations.Code AS LocationCode, Warehouses.Name AS WarehouseName, WarehouseInvoices.Description, WarehouseInvoices.TotalGrossAmount " + "\r\n";
            queryString = queryString + "       FROM        WarehouseInvoices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON WarehouseInvoices.EntryDate >= @FromDate AND WarehouseInvoices.EntryDate <= @ToDate AND WarehouseInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.WarehouseInvoice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = WarehouseInvoices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Warehouses ON WarehouseInvoices.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetWarehouseInvoiceIndexes", queryString);
        }

        private void GetWarehouseInvoiceViewDetails()
        {
            string queryString;

            queryString = " @WarehouseInvoiceID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      WarehouseInvoiceDetails.WarehouseInvoiceDetailID, WarehouseInvoiceDetails.WarehouseInvoiceID, WarehouseInvoiceDetails.StockTransferDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   StockTransferDetails.Quantity, 0 AS ListedPrice, 0 AS DiscountPercent, 0 AS UnitPrice, 0 AS VATPercent, 0 AS GrossPrice, 0 AS Amount, 0 AS VATAmount, 0 AS GrossAmount, WarehouseInvoiceDetails.Remarks" + "\r\n";
            queryString = queryString + "       FROM        WarehouseInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   StockTransferDetails ON WarehouseInvoiceDetails.WarehouseInvoiceID = @WarehouseInvoiceID AND WarehouseInvoiceDetails.StockTransferDetailID = StockTransferDetails.StockTransferDetailID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON StockTransferDetails.CommodityID = Commodities.CommodityID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetWarehouseInvoiceViewDetails", queryString);
        }

        private void GetPendingStockTransfers()
        {
            string queryString;

            queryString = " @StockTransferID Int, @AspUserID nvarchar(128), @LocationID Int, @StockTransferTypeID Int, @FromDate DateTime, @ToDate DateTime, @WarehouseInvoiceID Int, @StockTransferDetailIDs varchar(3999) " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@StockTransferID <> 0) " + "\r\n"; //IF @StockTransferID <> 0 THEN ALL OTHER Filter Parameters WILL BE NoNeeded. THIS CASE IS only USED TO MAKE ACCOUNTINVOICE AUTOMATICALLY FROM VEHICLEINVOICE
            queryString = queryString + "           " + this.GetPendingStockTransfersBuildSQL(true, false) + "\r\n";
            queryString = queryString + "       ELSE IF  (@StockTransferTypeID = " + (int)GlobalEnums.StockTransferTypeID.AllStockTransfer + ") " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransfersBuildSQL(false, false) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransfersBuildSQL(false, true) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPendingStockTransfers", queryString);
        }

        private string GetPendingStockTransfersBuildSQL(bool isStockTransferID, bool isStockTransferTypeID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@WarehouseInvoiceID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransfersBuildSQLA(isStockTransferID, isStockTransferTypeID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransfersBuildSQLA(isStockTransferID, isStockTransferTypeID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingStockTransfersBuildSQLA(bool isStockTransferID, bool isStockTransferTypeID, bool isWarehouseInvoiceID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@StockTransferDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransfersBuildSQLB(isStockTransferID, isStockTransferTypeID, isWarehouseInvoiceID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransfersBuildSQLB(isStockTransferID, isStockTransferTypeID, isWarehouseInvoiceID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }
        /// <summary>
        /// KTRA: SAVE UPDATE -- IsFinished KTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinished
        /// </summary>
        /// <param name="isStockTransferTypeID"></param>
        /// <param name="isWarehouseInvoiceID"></param>
        /// <param name="isStockTransferDetailIDs"></param>
        /// <returns></returns>
        private string GetPendingStockTransfersBuildSQLB(bool isStockTransferID, bool isStockTransferTypeID, bool isWarehouseInvoiceID, bool isStockTransferDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      StockTransferDetails.StockTransferDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.Name AS WarehouseName, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   StockTransferDetails.Quantity, 0 AS ListedPrice, 0 AS DiscountPercent, 0 AS UnitPrice, 0 AS VATPercent, 0 AS GrossPrice, 0 AS Amount, 0 AS VATAmount, 0 AS GrossAmount, CAST(1 AS bit) AS IsSelected " + "\r\n";
            queryString = queryString + "       FROM        StockTransferDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON StockTransferDetails.StockTransferID " + (isStockTransferID ? " = @StockTransferID " : " IN (SELECT StockTransfers.StockTransferID FROM StockTransfers WHERE StockTransfers.EntryDate >= @FromDate AND StockTransfers.EntryDate <= @ToDate AND StockTransfers.LocationID = @LocationID " + (isStockTransferTypeID ? " AND StockTransfers.StockTransferTypeID = @StockTransferTypeID" : "") + " AND StockTransfers.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID IN (" + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.VehicleTransfer + ", " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.PartTransfer + ") AND AccessControls.AccessLevel >= 1))      AND (StockTransferDetails.WarehouseInvoiceID IS NULL " + (isWarehouseInvoiceID ? " OR StockTransferDetails.WarehouseInvoiceID = @WarehouseInvoiceID" : "") + ")" + (isStockTransferDetailIDs ? " AND StockTransferDetails.StockTransferDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@StockTransferDetailIDs))" : "")) + " AND StockTransferDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 INNER JOIN " + "\r\n";
            queryString = queryString + "                   Warehouses ON StockTransferDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID" + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }


        private void WarehouseInvoiceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           UPDATE      StockTransferDetails" + "\r\n";
            queryString = queryString + "           SET         StockTransferDetails.WarehouseInvoiceID = WarehouseInvoiceDetails.WarehouseInvoiceID " + "\r\n";
            queryString = queryString + "           FROM        StockTransferDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                       WarehouseInvoiceDetails ON WarehouseInvoiceDetails.WarehouseInvoiceID = @EntityID AND StockTransferDetails.StockTransferDetailID = WarehouseInvoiceDetails.StockTransferDetailID " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           UPDATE      StockTransferDetails" + "\r\n";
            queryString = queryString + "           SET         WarehouseInvoiceID = NULL " + "\r\n";
            queryString = queryString + "           WHERE       WarehouseInvoiceID = @EntityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("WarehouseInvoiceSaveRelative", queryString);
        }

        private void WarehouseInvoicePostSaveValidate()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Phiếu chuyển kho chưa hoàn tất, hoặc ngày hóa đơn không hợp lệ. Ngày chuyển kho: ' + CAST(StockTransferDetails.EntryDate AS nvarchar) FROM WarehouseInvoiceDetails INNER JOIN StockTransferDetails ON WarehouseInvoiceDetails.WarehouseInvoiceID = @EntityID AND WarehouseInvoiceDetails.StockTransferDetailID = StockTransferDetails.StockTransferDetailID AND WarehouseInvoiceDetails.EntryDate < StockTransferDetails.EntryDate ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("WarehouseInvoicePostSaveValidate", queryArray);
        }



        private void WarehouseInvoiceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("WarehouseInvoices", "WarehouseInvoiceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.WarehouseInvoice));
            this.totalBikePortalsEntities.CreateTrigger("WarehouseInvoiceInitReference", simpleInitReference.CreateQuery());
        }


        private void WarehouseInvoiceSheet()
        {
            string queryString = " @WarehouseInvoiceID int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalWarehouseInvoiceID int    SET @LocalWarehouseInvoiceID = @WarehouseInvoiceID" + "\r\n";

            queryString = queryString + "       SELECT          WarehouseInvoices.WarehouseInvoiceID, WarehouseInvoices.EntryDate, WarehouseInvoices.Reference, WarehouseInvoices.VATInvoiceNo, WarehouseInvoices.VATInvoiceDate, WarehouseInvoices.VATInvoiceSeries, " + "\r\n";
            queryString = queryString + "                       Warehouses.WarehouseID, Warehouses.Name AS WarehouseName, Warehouses.OfficialName AS WarehouseOfficialName, Warehouses.VATCode, Warehouses.AddressNo, EntireTerritories.EntireName AS EntireTerritoryEntireName, PaymentTerms.Name AS PaymentTermName, " + "\r\n";
            queryString = queryString + "                       MINGoodsReceiptDetails.MINStockTransferTypeID, WarehouseInvoiceCollections.StockTransferDetailID, ISNULL(WarehouseInvoiceCollections.IsBonus, CAST(0 AS bit)) AS IsBonus, Commodities.CommodityID, Commodities.Code, Commodities.OfficialName + ' ' + ISNULL(WarehouseInvoiceCollections.Remarks, '') AS Name, Commodities.SalesUnit, WarehouseInvoiceCollections.LineDescription AS LineDescription, WarehouseInvoiceCollections.ChassisCode, WarehouseInvoiceCollections.EngineCode, WarehouseInvoiceCollections.ColorCode, WarehouseInvoiceCollections.ColorCodeName, " + "\r\n";
            queryString = queryString + "                       WarehouseInvoiceCollections.Quantity, WarehouseInvoiceCollections.UnitPrice, WarehouseInvoiceCollections.VATPercent, WarehouseInvoiceCollections.Amount, WarehouseInvoiceCollections.VATAmount, WarehouseInvoiceCollections.GrossAmount, " + "\r\n";
            queryString = queryString + "                       WarehouseInvoices.TotalQuantity, WarehouseInvoices.TotalAmount, WarehouseInvoices.TotalVATAmount, WarehouseInvoices.TotalGrossAmount, dbo.SayVND(WarehouseInvoices.TotalGrossAmount) AS TotalGrossAmountInWords, WarehouseInvoices.Description, " + "\r\n";
            queryString = queryString + "                       Locations.OfficialName AS LocationOfficialName, Locations.Address AS LocationAddress, Locations.Taxcode AS LocationTaxcode, Locations.Telephone AS LocationTelephone, Locations.Facsimile AS LocationFacsimile " + "\r\n";

            queryString = queryString + "       FROM            (SELECT StockTransferDetails.WarehouseInvoiceID, StockTransferDetails.StockTransferDetailID, StockTransferDetails.CommodityID, N'' AS LineDescription, StockTransferDetails.Quantity, StockTransferDetails.UnitPrice, StockTransferDetails.Amount, StockTransferDetails.VATPercent, StockTransferDetails.VATAmount, StockTransferDetails.GrossAmount, StockTransferDetails.Remarks, StockTransferDetails.IsBonus, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, ISNULL(ColorCodes.Name, GoodsReceiptDetails.ColorCode) AS ColorCodeName FROM StockTransferDetails LEFT JOIN GoodsReceiptDetails ON StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID LEFT JOIN ColorCodes ON GoodsReceiptDetails.CommodityID = ColorCodes.CommodityID AND GoodsReceiptDetails.ColorCode = ColorCodes.Code WHERE StockTransferDetails.WarehouseInvoiceID = @LocalWarehouseInvoiceID) AS WarehouseInvoiceCollections " + "\r\n";

            queryString = queryString + "                       INNER JOIN WarehouseInvoices ON WarehouseInvoiceCollections.WarehouseInvoiceID = WarehouseInvoices.WarehouseInvoiceID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON WarehouseInvoices.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ON Warehouses.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN PaymentTerms ON 1 = PaymentTerms.PaymentTermID " + "\r\n"; //WarehouseInvoices.PaymentTermID
            queryString = queryString + "                       INNER JOIN (SELECT MIN(StockTransferTypeID) AS MINStockTransferTypeID FROM StockTransferDetails WHERE WarehouseInvoiceID = @LocalWarehouseInvoiceID) MINGoodsReceiptDetails ON 1 = 1 " + "\r\n";

            queryString = queryString + "                       INNER JOIN Locations ON WarehouseInvoices.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "                       LEFT JOIN Commodities ON WarehouseInvoiceCollections.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            //this.totalBikePortalsEntities.CreateStoredProcedure("WarehouseInvoiceSheet", queryString);
        }
    }
}
