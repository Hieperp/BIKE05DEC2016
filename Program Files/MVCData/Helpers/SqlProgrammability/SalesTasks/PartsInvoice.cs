using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.SalesTasks
{
    public class PartsInvoice
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public PartsInvoice(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPartsInvoiceIndexes();

            this.GetCommoditiesInWarehouses("GetVehicleAvailables", true, false, false, false);
            this.GetCommoditiesInWarehouses("GetPartAvailables", false, true, false, false);
            this.GetCommoditiesInWarehouses("GetCommoditiesInWarehouses", false, true, true, false);
            this.GetCommoditiesInWarehouses("GetCommoditiesAvailables", true, true, false, false);

            this.GetCommoditiesInWarehouses("GetCommoditiesInWarehousesIncludeOutOfStock", false, true, true, true);

            this.GetPartsInvoiceViewDetails();
            this.PartsInvoiceSaveRelative();
            this.PartsInvoicePostSaveValidate();

            this.PartsInvoiceEditable();

        }


        private void GetPartsInvoiceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoices.SalesInvoiceID, CAST(SalesInvoices.EntryDate AS DATE) AS EntryDate, SalesInvoices.Reference, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, Commodities.Name AS CommodityName, ServiceContracts.LicensePlate, SalesInvoices.ServiceInvoiceID, ServiceInvoices.EntryDate AS ServiceDate, ServiceInvoices.Reference AS ServiceReference, SalesInvoices.TotalGrossAmount, ISNULL(ServiceInvoices.IsFinished, 1) AS IsFinished " + "\r\n";
            queryString = queryString + "       FROM        SalesInvoices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON SalesInvoices.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.PartsInvoice + " AND SalesInvoices.EntryDate >= @FromDate AND SalesInvoices.EntryDate <= @ToDate AND SalesInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.PartsInvoice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = SalesInvoices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON SalesInvoices.CustomerID = Customers.CustomerID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   ServiceContracts ON SalesInvoices.ServiceContractID = ServiceContracts.ServiceContractID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON ServiceContracts.CommodityID = Commodities.CommodityID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   SalesInvoices AS ServiceInvoices ON SalesInvoices.ServiceInvoiceID = ServiceInvoices.SalesInvoiceID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPartsInvoiceIndexes", queryString);
        }

        /// <summary>
        /// Get QuantityAvailable (Remaining) Commodities BY EVERY (WarehouseID, CommodityID)
        /// </summary>
        private void GetCommoditiesInWarehouses(string storedProcedureName, bool withCommoditiesInGoodsReceipts, bool withCommoditiesInWarehouses, bool getSavedData, bool includeCommoditiesOutOfStock)
        {
            //HIEN TAI, SU DUNG CHUNG CAC CAU SQL DE TAO RA NHIEU PHIEN BAN StoredProcedure, MUC DICH: NHAM DE QUAN LY CODE TUONG TU NHAU
            //VI VAY, CODE NAY TUAN THEO QUY UOC SAU: (CHI LA QUY UOC THOI, KHONG PHAI DIEU KIEN RANG BUOC GI CA)
            //withCommoditiesInGoodsReceipts = TRUE  and withCommoditiesInWarehouses = FALSE and getSavedData = FALSE: GetVehicleAvailables
            //withCommoditiesInGoodsReceipts = FALSE and withCommoditiesInWarehouses = TRUE  and getSavedData = FALSE: GetPartAvailables
            //withCommoditiesInGoodsReceipts = FALSE and withCommoditiesInWarehouses = TRUE  and getSavedData = TRUE: GetCommoditiesInWarehouses
            //withCommoditiesInGoodsReceipts = TRUE  and withCommoditiesInWarehouses = TRUE  and getSavedData = FALSE: GetCommoditiesAvailables
            //THEO QUY UOC NAY THI: getSavedData = TRUE ONLY WHEN: GetCommoditiesInWarehouses: DUOC SU DUNG TRONG SalesInvoice, StockTransfer, InventoryAdjustment
            //HAI TRUONG HOP: GetVehicleAvailables VA GetPartAvailables: DUOC SU DUNG TRONG VehicleTransferOrder VA PartTransferOrder (TAT NHIEN, SE CON DUOC SU DUNG TRONG NHUNG TRUONG HOP KHAC KHI KHONG YEU CAU LAY getSavedData, TUY NHIEN, HIEN TAI CHUA CO SU DUNG NOI NAO KHAC)
            //TRUONG HOP CUOI CUNG: GetCommoditiesAvailables: CUNG GIONG NHU GetVehicleAvailables VA GetPartAvailables, TUY NHIEN, NO BAO GOM CA Vehicle VA PartANDConsumable (get both Vehicles and Parts/ Consumables on the same view) (HIEN TAI, GetCommoditiesAvailables: CHUA DUOC SU DUNG O CHO NAO CA, CHI DE DANH SAU NAY CAN THIET THI SU DUNG THOI)

            
            //BO SUNG NGAY 08-JUN-2016: includeCommoditiesOutOfStock = TRUE: CHI DUY NHAT AP DUNG CHO GetCommoditiesInWarehousesIncludeOutOfStock. CAC T/H KHAC CHUA XEM XET, DO CHUA CO NHU CAU SU DUNG. NEU CO NHU CAU -> THI CO THE XEM XET LAI SQL QUERY


            string queryString = " @LocationID int, @EntryDate DateTime, @SearchText nvarchar(60) " + (getSavedData ? ", @SalesInvoiceID int, @StockTransferID int, @InventoryAdjustmentID int " : "") + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       DECLARE @Commodities TABLE (CommodityID int NOT NULL, Code nvarchar(50) NOT NULL, Name nvarchar(200) NOT NULL, GrossPrice decimal(18, 2) NOT NULL, CommodityTypeID int NOT NULL, CommodityCategoryID int NOT NULL)" + "\r\n";
            queryString = queryString + "       DECLARE @CommoditiesAvailable TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL, QuantityAvailable decimal(18, 2) NOT NULL)" + "\r\n";
            queryString = queryString + "       DECLARE @HasCommoditiesAvailable int SET @HasCommoditiesAvailable = 0" + "\r\n";

            queryString = queryString + "       INSERT INTO @Commodities SELECT CommodityID, Code, Name, GrossPrice, CommodityTypeID, CommodityCategoryID FROM Commodities WHERE CommodityTypeID IN (" + (withCommoditiesInGoodsReceipts ? "" + (int)GlobalEnums.CommodityTypeID.Vehicles : "") + (withCommoditiesInGoodsReceipts && withCommoditiesInWarehouses ? ", " : "") + (withCommoditiesInWarehouses ? (int)GlobalEnums.CommodityTypeID.Parts + ", " + (int)GlobalEnums.CommodityTypeID.Consumables : "") + ") AND (Code LIKE '%' + @SearchText + '%' OR Name LIKE '%' + @SearchText + '%') " + "\r\n";

            
            queryString = queryString + "       IF (@@ROWCOUNT > 0) " + "\r\n";
            queryString = queryString + "           " + this.GetCommoditiesInWarehousesBuildSQL(withCommoditiesInGoodsReceipts, withCommoditiesInWarehouses, getSavedData, includeCommoditiesOutOfStock) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetCommoditiesInWarehousesRETURNNothing() + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure(storedProcedureName, queryString);
        }

        private string GetCommoditiesInWarehousesGETAvailable(bool withCommoditiesInGoodsReceipts, bool withCommoditiesInWarehouses, bool getSavedData)
        {
            string queryString = "";

            if (withCommoditiesInGoodsReceipts)
            {//GET QuantityAvailable IN GoodsReceiptDetails FOR GlobalEnums.CommodityTypeID.Vehicles
                queryString = queryString + "               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "               SELECT          WarehouseID, CommodityID, ROUND(Quantity - QuantityIssue, 0) AS QuantityAvailable " + "\r\n";
                queryString = queryString + "               FROM            GoodsReceiptDetails " + "\r\n";
                queryString = queryString + "               WHERE           CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Vehicles + ") AND ROUND(Quantity - QuantityIssue, 0) > 0 AND WarehouseID IN (SELECT WarehouseID FROM Warehouses WHERE LocationID = @LocationID) AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
            }

            if (withCommoditiesInWarehouses)
            {
                //GET QuantityEndREC IN WarehouseJournal
                SqlProgrammability.StockTasks.Inventories inventories = new StockTasks.Inventories(this.totalBikePortalsEntities);

                queryString = queryString + "               DECLARE @WarehouseIDList varchar(35)        DECLARE @CommodityIDList varchar(3999) " + "\r\n";
                queryString = queryString + "               SELECT  @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID as varchar) FROM Warehouses WHERE LocationID = @LocationID FOR XML PATH('')) ,1,1,'') " + "\r\n";
                queryString = queryString + "               SELECT  @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID as varchar) FROM @Commodities FOR XML PATH('')) ,1,1,'') " + "\r\n";


                queryString = queryString + "               " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";

                queryString = queryString + "               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "               SELECT          WarehouseID, CommodityID, QuantityEndREC AS QuantityAvailable " + "\r\n";
                queryString = queryString + "               FROM            @WarehouseJournalTable " + "\r\n";

                queryString = queryString + "               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
            }

            if (getSavedData)
            {//GET SavedData
                queryString = queryString + "               IF (@SalesInvoiceID > 0) " + "\r\n";
                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "                               SELECT          WarehouseID, CommodityID, Quantity AS QuantityAvailable " + "\r\n";
                queryString = queryString + "                               FROM            SalesInvoiceDetails " + "\r\n";
                queryString = queryString + "                               WHERE           SalesInvoiceID = @SalesInvoiceID AND LocationID = @LocationID AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "                               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";

                queryString = queryString + "               IF (@StockTransferID > 0) " + "\r\n";
                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "                               SELECT          WarehouseID, CommodityID, Quantity AS QuantityAvailable " + "\r\n";
                queryString = queryString + "                               FROM            StockTransferDetails " + "\r\n";
                queryString = queryString + "                               WHERE           StockTransferID = @StockTransferID AND LocationID = @LocationID AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "                               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";

                queryString = queryString + "             IF (@InventoryAdjustmentID > 0) " + "\r\n";
                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "                               SELECT          WarehouseID, CommodityID, -Quantity AS QuantityAvailable " + "\r\n";
                queryString = queryString + "                               FROM            InventoryAdjustmentDetails " + "\r\n";
                queryString = queryString + "                               WHERE           InventoryAdjustmentID = @InventoryAdjustmentID AND LocationID = @LocationID AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "                               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";
            }


            return queryString;
        }

        private string GetCommoditiesInWarehousesBuildSQL(bool withCommoditiesInGoodsReceipts, bool withCommoditiesInWarehouses, bool getSavedData, bool includeCommoditiesOutOfStock)
        {
            string queryString = "                  BEGIN " + "\r\n";

            queryString = queryString + "               " + this.GetCommoditiesInWarehousesGETAvailable(withCommoditiesInGoodsReceipts, withCommoditiesInWarehouses, getSavedData) + "\r\n";


            if (includeCommoditiesOutOfStock)
            {
                queryString = queryString + "       DECLARE @CurrentWarehouseBalancePrice TABLE (CommodityID int NOT NULL, UnitPrice decimal(18, 2) NOT NULL)" + "\r\n";
                queryString = queryString + "       INSERT INTO @CurrentWarehouseBalancePrice SELECT CommodityID, UnitPrice FROM (SELECT EntryDate, CommodityID, UnitPrice, ROW_NUMBER() OVER (PARTITION BY CommodityID ORDER BY EntryDate DESC) AS RowNo FROM WarehouseBalancePrice WHERE CommodityID IN (SELECT CommodityID FROM @Commodities) AND EntryDate <= dbo.EOMONTHTIME(@EntryDate, 9999)) WarehouseBalancePriceWithRowNo WHERE RowNo = 1" + "\r\n";
            }


            if (!includeCommoditiesOutOfStock)
                queryString = queryString + "       IF (@HasCommoditiesAvailable > 0) " + "\r\n";

            queryString = queryString + "                   SELECT          " + (!includeCommoditiesOutOfStock ? "" : "TOP 50") + "Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + (!includeCommoditiesOutOfStock ? "Commodities.GrossPrice" : "ROUND(CAST(ISNULL(CurrentWarehouseBalancePrice.UnitPrice, 0) AS decimal(18, 2)) * (1 + CommodityCategories.VATPercent/100), " + (int)GlobalEnums.rndAmount + ") AS GrossPrice") + ", CommodityCategories.VATPercent, " + (!includeCommoditiesOutOfStock ? "" : "ISNULL(") + "Warehouses.WarehouseID" + (!includeCommoditiesOutOfStock ? "" : ", DEFAULTWarehouses.WarehouseID) AS WarehouseID") + ", " + (!includeCommoditiesOutOfStock ? "" : "ISNULL(") + "Warehouses.Code" + (!includeCommoditiesOutOfStock ? "" : ", DEFAULTWarehouses.Code)") + " AS WarehouseCode, " + (!includeCommoditiesOutOfStock ? "" : "ISNULL(") + "CommoditiesAvailable.QuantityAvailable" + (!includeCommoditiesOutOfStock ? "" : ", CAST(0 AS decimal(18, 2)) ) AS QuantityAvailable") + " \r\n";
            queryString = queryString + "                   FROM            @Commodities Commodities INNER JOIN " + "\r\n";
            queryString = queryString + "                                   CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID " + (!includeCommoditiesOutOfStock ? "INNER JOIN" : "LEFT JOIN") + "\r\n";
            queryString = queryString + "                                  (SELECT WarehouseID, CommodityID, SUM(QuantityAvailable) AS QuantityAvailable FROM @CommoditiesAvailable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON Commodities.CommodityID = CommoditiesAvailable.CommodityID " + (!includeCommoditiesOutOfStock ? "INNER JOIN" : "LEFT JOIN") + "\r\n";
            queryString = queryString + "                                   Warehouses ON CommoditiesAvailable.WarehouseID = Warehouses.WarehouseID " + "\r\n";


            if (includeCommoditiesOutOfStock)
            {
                queryString = queryString + "                               LEFT JOIN @CurrentWarehouseBalancePrice CurrentWarehouseBalancePrice ON Commodities.CommodityID = CurrentWarehouseBalancePrice.CommodityID " + "\r\n";
                queryString = queryString + "                               LEFT JOIN (SELECT TOP 1 WarehouseID, Code FROM Warehouses WHERE LocationID = @LocationID) DEFAULTWarehouses ON DEFAULTWarehouses.WarehouseID <> 0 " + "\r\n";
            }

            if (!includeCommoditiesOutOfStock)
            {
                queryString = queryString + "       ELSE " + "\r\n";
                queryString = queryString + "               " + this.GetCommoditiesInWarehousesRETURNNothing() + "\r\n";
            }

            queryString = queryString + "           END " + "\r\n";

            return queryString;
        }




        private string GetCommoditiesInWarehousesRETURNNothing()
        {
            string queryString = "                          BEGIN " + "\r\n";

            queryString = queryString + "                       SELECT      CommodityID, Code AS CommodityCode, Name AS CommodityName, Commodities.CommodityTypeID, Commodities.GrossPrice, 0.0 AS VATPercent, 0 AS WarehouseID, '' AS WarehouseCode, CAST(0 AS decimal(18, 2)) AS QuantityAvailable " + "\r\n";
            queryString = queryString + "                       FROM        @Commodities Commodities " + "\r\n";
            queryString = queryString + "                       WHERE       CommodityID IS NULL " + "\r\n"; //ALWAYS RETURN NOTHING

            queryString = queryString + "                   END " + "\r\n";

            return queryString;
        }




        #region X


        private void GetPartsInvoiceViewDetails()
        {
            string queryString;
            SqlProgrammability.StockTasks.Inventories inventories = new StockTasks.Inventories(this.totalBikePortalsEntities);

            queryString = " @SalesInvoiceID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @LocationID varchar(35)      DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";
            queryString = queryString + "       SELECT      @EntryDate = EntryDate, @LocationID = LocationID FROM SalesInvoices WHERE SalesInvoiceID = @SalesInvoiceID " + "\r\n";
            queryString = queryString + "       IF          @EntryDate IS NULL          SET @EntryDate = CONVERT(Datetime, '31/12/2000', 103)" + "\r\n";
            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID as varchar)  FROM Warehouses WHERE LocationID = @LocationID FOR XML PATH('')) ,1,1,'') " + "\r\n";//The best way is get the @WarehouseIDList from table SalesInvoiceDetails, but we don't want the stored procedure read from SalesInvoiceDetails to save the resource
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID as varchar)  FROM SalesInvoiceDetails WHERE SalesInvoiceID = @SalesInvoiceID FOR XML PATH('')) ,1,1,'') " + "\r\n";

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoiceDetails.SalesInvoiceDetailID, SalesInvoiceDetails.SalesInvoiceID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, SalesInvoiceDetails.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, " + "\r\n";
            queryString = queryString + "                   ROUND(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) + SalesInvoiceDetails.Quantity, 0) AS QuantityAvailable, SalesInvoiceDetails.Quantity, SalesInvoiceDetails.ListedPrice, SalesInvoiceDetails.DiscountPercent, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.GrossPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, SalesInvoiceDetails.IsBonus, SalesInvoiceDetails.IsWarrantyClaim, SalesInvoiceDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        SalesInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON SalesInvoiceDetails.SalesInvoiceID = @SalesInvoiceID AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON SalesInvoiceDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN" + "\r\n";
            queryString = queryString + "                  (SELECT WarehouseID, CommodityID, SUM(QuantityEndREC) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON SalesInvoiceDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND SalesInvoiceDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPartsInvoiceViewDetails", queryString);
        }

        private void PartsInvoiceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       EXEC        SalesInvoiceUpdateQuotation @EntityID, @SaveRelativeOption " + "\r\n";

            queryString = queryString + "       SET         @SaveRelativeOption = -@SaveRelativeOption" + "\r\n";
            queryString = queryString + "       EXEC        UpdateWarehouseBalance @SaveRelativeOption, 0, @EntityID, 0, 0 ";

            this.totalBikePortalsEntities.CreateStoredProcedure("PartsInvoiceSaveRelative", queryString);
        }

        private void PartsInvoicePostSaveValidate()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Service Date: ' + CAST(ServiceInvoices.EntryDate AS nvarchar) FROM SalesInvoices INNER JOIN SalesInvoices AS ServiceInvoices ON SalesInvoices.SalesInvoiceID = @EntityID AND SalesInvoices.ServiceInvoiceID = ServiceInvoices.SalesInvoiceID AND SalesInvoices.EntryDate < ServiceInvoices.EntryDate ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("PartsInvoicePostSaveValidate", queryArray);
        }







        private void PartsInvoiceEditable()
        {
            string[] queryArray = new string[2];//BE CAUTION: PartsInvoice SHOULD BE NOT ALLOWED TO CHANGE ServiceInvoiceID BY USER, IN ORDER THIS Procedure CAN CONTROL PartsInvoiceEditable VIA ServiceInvoiceID. IF USER REALLY NEED TO CHANGE: PLEASE USE THIS WORKARROUND INSTEAD: REQUEST USER TO DELETE THIS PartsInvoice, AND THEN CREATE NEW ONE WITH NEW FOREIGN KEY ServiceInvoiceID!

            queryArray[0] = "                 DECLARE @ServiceInvoiceID Int, @IsFinished Bit " + "\r\n";
            queryArray[0] = queryArray[0] + " SELECT TOP 1 @ServiceInvoiceID = ServiceInvoiceID, @IsFinished = IsFinished FROM SalesInvoices WHERE SalesInvoiceID = @EntityID " + "\r\n";
            queryArray[0] = queryArray[0] + " IF NOT @ServiceInvoiceID IS NULL" + "\r\n";
            queryArray[0] = queryArray[0] + "       SELECT TOP 1 @FoundEntity = SalesInvoiceID FROM SalesInvoices WHERE SalesInvoiceID = @ServiceInvoiceID AND IsFinished = 1 " + "\r\n";
            queryArray[0] = queryArray[0] + " ELSE IF @IsFinished = 1 " + "\r\n";
            queryArray[0] = queryArray[0] + "       SELECT @FoundEntity = @EntityID " + "\r\n";

            queryArray[1] = " SELECT TOP 1 @FoundEntity = SalesInvoiceID FROM SalesInvoiceDetails WHERE SalesInvoiceID = @EntityID AND NOT AccountInvoiceID IS NULL ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("PartsInvoiceEditable", queryArray);
        }


        #endregion
    }
}
