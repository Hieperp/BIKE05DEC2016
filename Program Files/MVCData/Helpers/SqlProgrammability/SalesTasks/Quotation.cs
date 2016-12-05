using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.SalesTasks
{
    public class Quotation
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public Quotation(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetQuotationViewDetails();
            this.QuotationSaveRelative();
            this.QuotationPostSaveValidate();

            this.QuotationEditable();

            this.QuotationInitReference();
            this.SearchQuotations();
        }




        //Van de warehouse id, code, name; can phai xem xet lai!LEMINHHIEP
        private void GetQuotationViewDetails()
        {
            string queryString;
            SqlProgrammability.StockTasks.Inventories inventories = new StockTasks.Inventories(this.totalBikePortalsEntities);
            

            queryString = " @QuotationID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @LocationID varchar(35)      DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";
            queryString = queryString + "       SELECT      @EntryDate = EntryDate, @LocationID = LocationID FROM Quotations WHERE QuotationID = @QuotationID " + "\r\n";
            queryString = queryString + "       IF          @EntryDate IS NULL          SET @EntryDate = CONVERT(Datetime, '31/12/2000', 103)" + "\r\n";
            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID as varchar)  FROM Warehouses WHERE LocationID = @LocationID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID as varchar)  FROM QuotationDetails WHERE QuotationID = @QuotationID FOR XML PATH('')) ,1,1,'') " + "\r\n";

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";

            queryString = queryString + "       SELECT      QuotationDetails.QuotationDetailID, QuotationDetails.QuotationID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   CommoditiesAvailable.WarehouseID, CommoditiesAvailable.WarehouseCode, ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS QuantityAvailable, QuotationDetails.Quantity, QuotationDetails.QuantityInvoice, QuotationDetails.ListedPrice, QuotationDetails.DiscountPercent, QuotationDetails.UnitPrice, QuotationDetails.VATPercent, QuotationDetails.GrossPrice, QuotationDetails.Amount, QuotationDetails.VATAmount, QuotationDetails.GrossAmount, QuotationDetails.IsBonus, QuotationDetails.IsWarrantyClaim, QuotationDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        QuotationDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON QuotationDetails.QuotationID = @QuotationID AND QuotationDetails.CommodityID = Commodities.CommodityID LEFT JOIN" + "\r\n";
            queryString = queryString + "                  (SELECT CommodityID, MIN(WarehouseID) AS WarehouseID, MIN(WarehouseCode) AS WarehouseCode, SUM(QuantityEndREC) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY CommodityID) CommoditiesAvailable ON QuotationDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetQuotationViewDetails", queryString);
        }

        private void QuotationSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            

            this.totalBikePortalsEntities.CreateStoredProcedure("QuotationSaveRelative", queryString);
        }

        private void QuotationPostSaveValidate()
        {
            //string[] queryArray = new string[2];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsReceipts.GoodsReceiptID FROM PurchaseOrders INNER JOIN PurchaseOrderDetails ON PurchaseOrders.PurchaseOrderID = PurchaseOrderDetails.PurchaseOrderID INNER JOIN GoodsReceiptDetails ON PurchaseOrderDetails.PurchaseOrderDetailID = GoodsReceiptDetails.PurchaseOrderDetailID INNER JOIN GoodsReceipts ON GoodsReceiptDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID AND GoodsReceipts.EntryDate < PurchaseOrders.EntryDate ";
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = PurchaseOrderID FROM PurchaseOrderDetail WHERE (ROUND(Quantity - QuantityInvoice, 0) < 0) ";

            string[] queryArray = new string[0];
            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("QuotationPostSaveValidate", queryArray);
        }



        private void QuotationEditable()
        {
            string[] queryArray = new string[2];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = QuotationDetails.QuotationID FROM ServiceContracts INNER JOIN QuotationDetails ON ServiceContracts.QuotationDetailID = QuotationDetails.QuotationDetailID WHERE QuotationDetails.QuotationID = @EntityID ";
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = QuotationID FROM Quotations WHERE ServiceInvoiceID = @EntityID ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("QuotationEditable");
        }



        private void QuotationInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("Quotations", "QuotationID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.Quotation));
            this.totalBikePortalsEntities.CreateTrigger("QuotationInitReference", simpleInitReference.CreateQuery());
        }


        private void SearchQuotations()
        {
            string querySQL = " SELECT Quotations.QuotationID, Quotations.EntryDate, Quotations.Reference, Quotations.CustomerID, Quotations.ServiceContractID, ServiceContracts.Reference AS ServiceContractReference, ServiceContracts.CommodityID AS ServiceContractCommodityID, ServiceContracts.LicensePlate, ServiceContracts.ChassisCode, ServiceContracts.EngineCode, ServiceContracts.ColorCode, ServiceContracts.PurchaseDate, ServiceContracts.AgentName FROM Quotations INNER JOIN ServiceContracts ON Quotations.ServiceContractID = ServiceContracts.ServiceContractID WHERE Quotations.LocationID = @LocationID AND Quotations.InActive = 0 AND (Quotations.QuotationID = @QuotationID OR (@IsFinished = -1 OR Quotations.IsFinished = @IsFinished)) ";

            string queryString = " @LocationID int, @QuotationID int, @SearchText nvarchar(100), @IsFinished int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @Quotations TABLE (QuotationID int, EntryDate datetime NOT NULL, Reference nvarchar(10) NULL, CustomerID int NOT NULL, ServiceContractID int, ServiceContractReference nvarchar(10) NULL, ServiceContractCommodityID int NOT NULL, LicensePlate nvarchar(60) NULL, ChassisCode nvarchar(60) NULL, EngineCode nvarchar(60) NULL, ColorCode nvarchar(60) NULL, PurchaseDate datetime NULL, AgentName nvarchar(100) NULL)" + "\r\n";


            queryString = queryString + "       IF (@SearchText = '') " + "\r\n";
            queryString = queryString + "           INSERT INTO @Quotations " + querySQL + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               INSERT INTO @Quotations " + querySQL + " AND ServiceContracts.LicensePlate LIKE '%' + @SearchText + '%' " + "\r\n";
            queryString = queryString + "               IF (@@ROWCOUNT <= 0) " + "\r\n";
            queryString = queryString + "                   INSERT INTO @Quotations " + querySQL + " AND (ServiceContracts.ChassisCode LIKE '%' + @SearchText + '%' OR ServiceContracts.EngineCode LIKE '%' + @SearchText + '%') " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       SELECT  Quotations.QuotationID, Quotations.EntryDate, Quotations.Reference, " + "\r\n";
            queryString = queryString + "               Quotations.CustomerID, Customers.Name AS CustomerName, Customers.Birthday AS CustomerBirthday, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "               Quotations.ServiceContractID, Quotations.ServiceContractReference, Quotations.ServiceContractCommodityID, Commodities.Code AS ServiceContractCommodityCode, Commodities.Name AS ServiceContractCommodityName, Quotations.LicensePlate AS ServiceContractLicensePlate, Quotations.ColorCode AS ServiceContractColorCode, Quotations.ChassisCode AS ServiceContractChassisCode, Quotations.EngineCode AS ServiceContractEngineCode, Quotations.PurchaseDate AS ServiceContractPurchaseDate, Quotations.AgentName AS ServiceContractAgentName " + "\r\n";
            queryString = queryString + "       FROM    @Quotations Quotations INNER JOIN " + "\r\n";
            queryString = queryString + "               Commodities ON Quotations.ServiceContractCommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryString = queryString + "               Customers ON Quotations.CustomerID = Customers.CustomerID INNER JOIN " + "\r\n";
            queryString = queryString + "               EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";
            

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("SearchQuotations", queryString);
        }

    }
}
