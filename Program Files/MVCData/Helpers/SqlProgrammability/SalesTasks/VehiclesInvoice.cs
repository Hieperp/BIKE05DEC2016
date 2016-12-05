using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.SalesTasks
{
    public class VehiclesInvoice
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public VehiclesInvoice(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetVehiclesInvoiceIndexes();

            this.GetCommoditiesInGoodsReceipts();

            this.SalesInvoiceUpdateQuotation();

            this.GetVehiclesInvoiceViewDetails();
            this.VehiclesInvoiceSaveRelative();
            this.VehiclesInvoicePostSaveValidate();

            this.VehiclesInvoiceEditable();
            this.VehiclesInvoiceDeletable();

            this.SalesInvoiceInitReference();
        }

        private void GetVehiclesInvoiceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoices.SalesInvoiceID, CAST(SalesInvoices.EntryDate AS DATE) AS EntryDate, SalesInvoices.Reference, SalesInvoices.VATInvoiceNo, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, Commodities.Name AS CommodityName, SalesInvoiceDetails.GrossAmount " + "\r\n";
            queryString = queryString + "       FROM        SalesInvoices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON SalesInvoices.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice + " AND SalesInvoices.EntryDate >= @FromDate AND SalesInvoices.EntryDate <= @ToDate AND SalesInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.VehiclesInvoice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = SalesInvoices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON SalesInvoices.CustomerID = Customers.CustomerID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails ON SalesInvoices.SalesInvoiceID = SalesInvoiceDetails.SalesInvoiceID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON SalesInvoiceDetails.CommodityID = Commodities.CommodityID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetVehiclesInvoiceIndexes", queryString);
        }

        /// <summary>
        /// Get QuantityAvailable (Remaining) Commodities BY EVERY GoodsReceiptDetailID (THIS MEANS: BY EVERY GoodsReceiptDetails LINE)
        /// </summary>
        private void GetCommoditiesInGoodsReceipts()
        {
            string queryString = " @LocationID int, @SearchText nvarchar(60), @SalesInvoiceID int, @StockTransferID int, @InventoryAdjustmentID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       DECLARE @WarehouseFilter TABLE (WarehouseID int NOT NULL) " + "\r\n";
            queryString = queryString + "       INSERT INTO @WarehouseFilter SELECT WarehouseID FROM Warehouses WHERE LocationID = @LocationID " + "\r\n";
            
            queryString = queryString + "       DECLARE @HasSavedData int SET @HasSavedData = 0" + "\r\n";
            queryString = queryString + "       DECLARE @SavedData TABLE (GoodsReceiptDetailID int NOT NULL, QuantityAvailable decimal(18, 2) NOT NULL)" + "\r\n";
            queryString = queryString + "       DECLARE @Commodities TABLE (CommodityID int NOT NULL, Code nvarchar(50) NOT NULL, Name nvarchar(200) NOT NULL, GrossPrice decimal(18, 2) NOT NULL, CommodityTypeID int NOT NULL, CommodityCategoryID int NOT NULL)" + "\r\n";

            queryString = queryString + "       INSERT INTO @Commodities SELECT CommodityID, Code, Name, GrossPrice, CommodityTypeID, CommodityCategoryID FROM Commodities WHERE CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Vehicles + ") AND (Code LIKE '%' + @SearchText + '%' OR Name LIKE '%' + @SearchText + '%') " + "\r\n";

            queryString = queryString + "       IF (@@ROWCOUNT > 0) " + "\r\n"; //Search by code/ name: @Commodities: Commodities HAS FOUND
            queryString = queryString + "           " + this.GetCommoditiesInGoodsReceiptsBuildSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n"; //@@ROWCOUNT <= 0: Try to search by ChassisCode, EngineCode, ColorCode from GoodsReceiptDetails
            queryString = queryString + "           " + this.GetCommoditiesInGoodsReceiptsBuildSQL(false) + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetCommoditiesInGoodsReceipts", queryString);
        }

        private string GetCommoditiesInGoodsReceiptsBuildSQL(bool hasFoundCommoditiesByName)
        {
            string queryString = "                  BEGIN " + "\r\n";

            queryString = queryString + "               " + this.GetCommoditiesInGoodsReceiptsSearchSavedDataBuildSQL(hasFoundCommoditiesByName) + "\r\n";

            queryString = queryString + "               IF (@HasSavedData > 0) " + "\r\n";
            queryString = queryString + "                   " + this.GetCommoditiesInGoodsReceiptsWITHSavedDataBuildSQL(hasFoundCommoditiesByName) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   " + this.GetCommoditiesInGoodsReceiptsWITHOUTSavedDataBuildSQL(hasFoundCommoditiesByName) + "\r\n";

            queryString = queryString + "           END " + "\r\n";

            return queryString;
        }


        private string GetCommoditiesInGoodsReceiptsSearchSavedDataBuildSQL(bool hasFoundCommoditiesByName)
        {
            string queryString = "                      IF (@SalesInvoiceID > 0) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                               INSERT INTO     @SavedData (GoodsReceiptDetailID, QuantityAvailable) " + "\r\n";
            queryString = queryString + "                               SELECT          GoodsReceiptDetailID, Quantity AS QuantityAvailable " + "\r\n";
            queryString = queryString + "                               FROM            SalesInvoiceDetails " + "\r\n";
            queryString = queryString + "                               WHERE           SalesInvoiceID = @SalesInvoiceID AND LocationID = @LocationID " + (hasFoundCommoditiesByName ? " AND CommodityID IN (SELECT CommodityID FROM @Commodities) " : "") + "\r\n";

            queryString = queryString + "                               SET             @HasSavedData = @@ROWCOUNT " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               IF (@StockTransferID > 0) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                               INSERT INTO     @SavedData (GoodsReceiptDetailID, QuantityAvailable) " + "\r\n";
            queryString = queryString + "                               SELECT          GoodsReceiptDetailID, Quantity AS QuantityAvailable " + "\r\n";
            queryString = queryString + "                               FROM            StockTransferDetails " + "\r\n";
            queryString = queryString + "                               WHERE           StockTransferID = @StockTransferID AND LocationID = @LocationID " + (hasFoundCommoditiesByName ? " AND CommodityID IN (SELECT CommodityID FROM @Commodities) " : "") + "\r\n";

            queryString = queryString + "                               SET             @HasSavedData = @@ROWCOUNT " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            //queryString = queryString + "               IF (@InventoryAdjustmentID > 0) " + "\r\n";

            return queryString;
        }


        private string GetCommoditiesInGoodsReceiptsWITHOUTSavedDataBuildSQL(bool hasFoundCommoditiesByName)
        {
            string queryString = "                          BEGIN " + "\r\n";
            queryString = queryString + "                       SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.SupplierID, GoodsReceiptDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Commodities.GrossPrice, CommodityCategories.VATPercent, GoodsReceiptDetails.WarehouseID, Warehouses.Code AS WarehouseCode, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue, 0) AS QuantityAvailable, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode " + "\r\n";
            queryString = queryString + "                       FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                   " + (hasFoundCommoditiesByName ? "@Commodities" : "") + " Commodities ON " + this.GetCommoditiesInGoodsReceiptsWarehouseFilter("GoodsReceiptDetails") + " AND ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue, 0) > 0 AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + (hasFoundCommoditiesByName ? "" : " AND Commodities.CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Vehicles + ") AND (GoodsReceiptDetails.ChassisCode LIKE '%' + @SearchText + '%' OR GoodsReceiptDetails.EngineCode LIKE '%' + @SearchText + '%' OR GoodsReceiptDetails.ColorCode LIKE '%' + @SearchText + '%') ") + " INNER JOIN " + "\r\n";
            queryString = queryString + "                                   Warehouses ON GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID INNER JOIN " + "\r\n";
            queryString = queryString + "                                   CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            return queryString;
        }

        private string GetCommoditiesInGoodsReceiptsWITHSavedDataBuildSQL(bool hasFoundCommoditiesByName)
        {
            string queryString = "                          BEGIN " + "\r\n";

            queryString = queryString + "                       SELECT      GoodsReceiptCOLLECTION.GoodsReceiptDetailID, GoodsReceiptCOLLECTION.SupplierID, GoodsReceiptCOLLECTION.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Commodities.GrossPrice, CommodityCategories.VATPercent, GoodsReceiptCOLLECTION.WarehouseID, Warehouses.Code AS WarehouseCode, GoodsReceiptCOLLECTION.QuantityAvailable AS QuantityAvailable, GoodsReceiptCOLLECTION.ChassisCode, GoodsReceiptCOLLECTION.EngineCode, GoodsReceiptCOLLECTION.ColorCode " + "\r\n";
            queryString = queryString + "                       FROM       (" + "\r\n";
            queryString = queryString + "                                   SELECT      GoodsReceiptDetailID, MAX(SupplierID) AS SupplierID, MAX(CommodityID) AS CommodityID, MAX(WarehouseID) AS WarehouseID, SUM(QuantityAvailable) AS QuantityAvailable, MAX(ChassisCode) AS ChassisCode, MAX(EngineCode) AS EngineCode, MAX(ColorCode) AS ColorCode " + "\r\n";
            queryString = queryString + "                                   FROM       ( " + "\r\n";
            queryString = queryString + "                                               SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.SupplierID, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.WarehouseID, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue, 0) AS QuantityAvailable, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode " + "\r\n";
            queryString = queryString + "                                               FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                               WHERE       " + this.GetCommoditiesInGoodsReceiptsWarehouseFilter("GoodsReceiptDetails") + " AND ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue, 0) > 0 AND " + (hasFoundCommoditiesByName ? "CommodityID IN (SELECT CommodityID FROM @Commodities)" : "(GoodsReceiptDetails.ChassisCode LIKE '%' + @SearchText + '%' OR GoodsReceiptDetails.EngineCode LIKE '%' + @SearchText + '%' OR GoodsReceiptDetails.ColorCode LIKE '%' + @SearchText + '%')") + "\r\n";

            queryString = queryString + "                                               UNION ALL " + "\r\n";

            queryString = queryString + "                                               SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.SupplierID, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.WarehouseID, SavedData.QuantityAvailable, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode " + "\r\n";
            queryString = queryString + "                                               FROM        @SavedData SavedData INNER JOIN " + "\r\n";
            queryString = queryString + "                                                           GoodsReceiptDetails ON SavedData.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + (hasFoundCommoditiesByName ? "" : "AND (GoodsReceiptDetails.ChassisCode LIKE '%' + @SearchText + '%' OR GoodsReceiptDetails.EngineCode LIKE '%' + @SearchText + '%' OR GoodsReceiptDetails.ColorCode LIKE '%' + @SearchText + '%') ") + "\r\n";
            queryString = queryString + "                                              )GoodsReceiptUNION " + "\r\n";
            queryString = queryString + "                                   GROUP BY    GoodsReceiptDetailID" + "\r\n";
            queryString = queryString + "                                  )GoodsReceiptCOLLECTION INNER JOIN " + "\r\n";
            queryString = queryString + "                                   " + (hasFoundCommoditiesByName ? "@Commodities" : "") + " Commodities ON GoodsReceiptCOLLECTION.CommodityID = Commodities.CommodityID " + (hasFoundCommoditiesByName ? "" : " AND Commodities.CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Vehicles + ")") + " INNER JOIN " + "\r\n";
            queryString = queryString + "                                   Warehouses ON GoodsReceiptCOLLECTION.WarehouseID = Warehouses.WarehouseID INNER JOIN " + "\r\n";
            queryString = queryString + "                                   CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID " + "\r\n";

            queryString = queryString + "                   END " + "\r\n";
            
            return queryString;
        }


        private string GetCommoditiesInGoodsReceiptsWarehouseFilter(string tableName)
        {
            return (tableName != "" ? tableName + "." : "") + "WarehouseID IN (SELECT WarehouseID FROM @WarehouseFilter) " + "\r\n";
        }


        private void GetVehiclesInvoiceViewDetails()
        {
            string queryString;

            queryString = " @SalesInvoiceID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoiceDetails.SalesInvoiceDetailID, SalesInvoiceDetails.SalesInvoiceID, SalesInvoiceDetails.GoodsReceiptDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, SalesInvoiceDetails.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue + SalesInvoiceDetails.Quantity, 0) AS QuantityAvailable, SalesInvoiceDetails.Quantity, SalesInvoiceDetails.ListedPrice, SalesInvoiceDetails.DiscountPercent, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.GrossPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, SalesInvoiceDetails.IsBonus, SalesInvoiceDetails.IsWarrantyClaim, SalesInvoiceDetails.Remarks" + "\r\n";
            queryString = queryString + "       FROM        SalesInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON SalesInvoiceDetails.SalesInvoiceID = @SalesInvoiceID AND SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON GoodsReceiptDetails.CommodityID = Commodities.CommodityID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetVehiclesInvoiceViewDetails", queryString);
        }


        private void SalesInvoiceUpdateQuotation()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       UPDATE      QuotationDetails" + "\r\n";
            queryString = queryString + "       SET         QuantityInvoice = ROUND(QuotationDetails.QuantityInvoice + SalesInvoiceDetails.Quantity * @SaveRelativeOption, 0)" + "\r\n";
            queryString = queryString + "       FROM       (SELECT QuotationDetailID, SUM(Quantity) AS Quantity FROM SalesInvoiceDetails WHERE SalesInvoiceID = @EntityID AND QuotationDetailID IS NOT NULL GROUP BY QuotationDetailID) SalesInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   QuotationDetails ON SalesInvoiceDetails.QuotationDetailID = QuotationDetails.QuotationDetailID" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("SalesInvoiceUpdateQuotation", queryString);
        }

        private void VehiclesInvoiceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       EXEC        SalesInvoiceUpdateQuotation @EntityID, @SaveRelativeOption " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsReceiptDetails" + "\r\n";
            queryString = queryString + "       SET         QuantityIssue = ROUND(GoodsReceiptDetails.QuantityIssue + SalesInvoiceDetails.Quantity * @SaveRelativeOption, 0)" + "\r\n";
            queryString = queryString + "       FROM       (SELECT GoodsReceiptDetailID, SUM(Quantity) AS Quantity FROM SalesInvoiceDetails WHERE SalesInvoiceID = @EntityID GROUP BY GoodsReceiptDetailID) SalesInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID ; " + "\r\n";

            queryString = queryString + "       IF @SaveRelativeOption = 1 " + "\r\n"; //NEED TO UPDATE ONLY WHEN Update
            queryString = queryString + "                   UPDATE      SalesInvoices SET SalesInvoices.CustomerID = VehiclesInvoices.CustomerID FROM SalesInvoices INNER JOIN SalesInvoices AS VehiclesInvoices ON VehiclesInvoices.SalesInvoiceID = @EntityID AND SalesInvoices.ServiceInvoiceID = VehiclesInvoices.SalesInvoiceID " + "\r\n";

            //////queryString = queryString + "       THROW 60000, N'My Exception: Le Minh Hiep--throw', 1 " + "\r\n"; //RAISERROR (N'My Exception: Le Minh Hiep', 16, 1) 

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("VehiclesInvoiceSaveRelative", queryString);
        }

        private void VehiclesInvoicePostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Warehouse Date: ' + CAST(GoodsReceiptDetails.EntryDate AS nvarchar) FROM SalesInvoiceDetails INNER JOIN GoodsReceiptDetails ON SalesInvoiceDetails.SalesInvoiceID = @EntityID AND SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID AND SalesInvoiceDetails.EntryDate < GoodsReceiptDetails.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = 'Over Quantity: ' + CAST(ROUND(Quantity - QuantityIssue, 0) AS nvarchar) FROM GoodsReceiptDetails WHERE (ROUND(Quantity - QuantityIssue, 0) < 0) ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("VehiclesInvoicePostSaveValidate", queryArray);
        }


        private void VehiclesInvoiceEditable()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesInvoiceID FROM SalesInvoices WHERE SalesInvoiceID = @EntityID AND IsFinished = 1 ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = ServiceContractID FROM ServiceContracts WHERE SalesInvoiceDetailID IN (SELECT SalesInvoiceDetailID FROM SalesInvoiceDetails WHERE SalesInvoiceID = @EntityID) ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = SalesInvoiceID FROM SalesInvoiceDetails WHERE SalesInvoiceID = @EntityID AND NOT AccountInvoiceID IS NULL ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("VehiclesInvoiceEditable", queryArray);
        }

        private void VehiclesInvoiceDeletable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesInvoiceID FROM SalesInvoices WHERE ServiceInvoiceID = @EntityID ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("VehiclesInvoiceDeletable", queryArray);
        }



        private void SalesInvoiceInitReference()
        {
            SalesInvoiceInitReference simpleInitReference = new SalesInvoiceInitReference("SalesInvoices", "SalesInvoiceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.SalesInvoice));
            this.totalBikePortalsEntities.CreateTrigger("SalesInvoiceInitReference", simpleInitReference.CreateQuery());
        }
    }
}
