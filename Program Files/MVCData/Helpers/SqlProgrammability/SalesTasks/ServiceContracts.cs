using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.SalesTasks
{
    public class ServiceContracts
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public ServiceContracts(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetServiceContractIndexes();

            this.ServiceContractGetVehiclesInvoices();
            this.SearchServiceContracts();

            this.ServiceContractSaveRelative();
            this.ServiceContractPostSaveValidate();

            this.ServicesContractDeletable();

            this.ServiceContractInitReference();
        }

        private void GetServiceContractIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      ServiceContracts.ServiceContractID, CAST(ServiceContracts.EntryDate AS DATE) AS EntryDate, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, Commodities.Name AS CommodityName, ServiceContracts.ChassisCode, ServiceContracts.EngineCode, ServiceContracts.LicensePlate, ServiceContracts.AgentName, ServiceContracts.PurchaseDate " + "\r\n";
            queryString = queryString + "       FROM        ServiceContracts INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON ServiceContracts.EntryDate >= @FromDate AND ServiceContracts.EntryDate <= @ToDate AND ServiceContracts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.ServiceContract + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = ServiceContracts.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON ServiceContracts.CustomerID = Customers.CustomerID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON ServiceContracts.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetServiceContractIndexes", queryString);
        }

        private void ServiceContractGetVehiclesInvoices()
        {
            string queryString = " @LocationID int, @SearchText nvarchar(60), @SalesInvoiceID int, @ServiceContractID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       DECLARE @HasFoundCommoditiesByName int SET @HasFoundCommoditiesByName = 0" + "\r\n";
            queryString = queryString + "       DECLARE @HasSavedData int SET @HasSavedData = 0" + "\r\n";
            queryString = queryString + "       DECLARE @SavedData TABLE (SalesInvoiceDetailID int NOT NULL)" + "\r\n";
            queryString = queryString + "       DECLARE @Commodities TABLE (CommodityID int NOT NULL, Code nvarchar(50) NOT NULL, Name nvarchar(200) NOT NULL, CommodityCategoryID int NOT NULL)" + "\r\n";

            queryString = queryString + "       IF (@SearchText <> '') " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               INSERT INTO @Commodities SELECT CommodityID, Code, Name, CommodityCategoryID FROM Commodities WHERE CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Vehicles + ") AND (Code LIKE '%' + @SearchText + '%' OR Name LIKE '%' + @SearchText + '%') " + "\r\n";
            queryString = queryString + "               SET @HasFoundCommoditiesByName = @@ROWCOUNT " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       IF (@HasFoundCommoditiesByName > 0) " + "\r\n"; //Search by code/ name: @Commodities: Commodities HAS FOUND
            queryString = queryString + "           " + this.ServiceContractGetVehiclesInvoicesBuildSQL(true, false) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n"; //@HasFoundCommoditiesByName <= 0: Try to search by ChassisCode, EngineCode from GoodsReceiptDetails
            queryString = queryString + "           IF (@SearchText <> '') " + "\r\n";
            queryString = queryString + "               " + this.ServiceContractGetVehiclesInvoicesBuildSQL(false, true) + "\r\n";
            queryString = queryString + "           ELSE " + "\r\n";
            queryString = queryString + "               " + this.ServiceContractGetVehiclesInvoicesBuildSQL(false, false) + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("ServiceContractGetVehiclesInvoices", queryString);
        }

        private string ServiceContractGetVehiclesInvoicesBuildSQL(bool hasFoundCommoditiesByName, bool searchChassisEngineCode)
        {
            string queryString = "                  BEGIN " + "\r\n";

            queryString = queryString + "               " + this.ServiceContractGetVehiclesInvoicesSearchSavedDataBuildSQL(hasFoundCommoditiesByName) + "\r\n";

            queryString = queryString + "               IF (@HasSavedData > 0) " + "\r\n";
            queryString = queryString + "                   " + this.ServiceContractGetVehiclesInvoicesReturnDbSetBuildSQL(hasFoundCommoditiesByName, searchChassisEngineCode, true) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   " + this.ServiceContractGetVehiclesInvoicesReturnDbSetBuildSQL(hasFoundCommoditiesByName, searchChassisEngineCode, false) + "\r\n";

            queryString = queryString + "           END " + "\r\n";

            return queryString;
        }


        private string ServiceContractGetVehiclesInvoicesSearchSavedDataBuildSQL(bool hasFoundCommoditiesByName)
        {
            string queryString = "                      IF (@ServiceContractID > 0) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                               INSERT INTO     @SavedData (SalesInvoiceDetailID) " + "\r\n";
            queryString = queryString + "                               SELECT          SalesInvoiceDetailID " + "\r\n";
            queryString = queryString + "                               FROM            ServiceContracts " + "\r\n";
            queryString = queryString + "                               WHERE           LocationID = @LocationID AND ServiceContractID = @ServiceContractID " + (hasFoundCommoditiesByName ? " AND CommodityID IN (SELECT CommodityID FROM @Commodities) " : "") + "\r\n";

            queryString = queryString + "                               SET             @HasSavedData = @@ROWCOUNT " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            return queryString;
        }


        private string ServiceContractGetVehiclesInvoicesReturnDbSetBuildSQL(bool hasFoundCommoditiesByName, bool searchChassisEngineCode, bool hasSavedData)
        {
            string queryString = "                          BEGIN " + "\r\n";

            queryString = queryString + "                       SELECT      SalesInvoiceDetails.SalesInvoiceDetailID, SalesInvoices.EntryDate, Customers.CustomerID, Customers.Name AS CustomerName, Customers.Birthday AS CustomerBirthday, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, CommodityCategories.LimitedMonthWarranty, CommodityCategories.LimitedKilometreWarranty, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, SalesInvoices.EntryDate AS BeginningDate, DATEADD(month, CommodityCategories.LimitedMonthWarranty, SalesInvoices.EntryDate) AS EndingDate " + "\r\n";
            queryString = queryString + "                       FROM        SalesInvoiceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                   " + (hasFoundCommoditiesByName ? "@Commodities" : "") + " Commodities ON SalesInvoiceDetails.LocationID = @LocationID AND (@SalesInvoiceID = 0 OR SalesInvoiceDetails.SalesInvoiceID = @SalesInvoiceID) AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID AND (ROUND(SalesInvoiceDetails.Quantity - SalesInvoiceDetails.QuantityContract, 0) > 0 " + (hasSavedData ? " OR SalesInvoiceDetails.SalesInvoiceDetailID IN (SELECT SalesInvoiceDetailID FROM @SavedData) " : "") + ") INNER JOIN " + "\r\n";
            queryString = queryString + "                                   SalesInvoices ON SalesInvoices.SalesInvoiceID = SalesInvoiceDetails.SalesInvoiceID INNER JOIN " + "\r\n";
            queryString = queryString + "                                   Customers ON SalesInvoices.CustomerID = Customers.CustomerID AND Customers.CustomerCategoryID <> 1 INNER JOIN " + "\r\n";
            queryString = queryString + "                                   EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID INNER JOIN " + "\r\n";
            queryString = queryString + "                                   CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID INNER JOIN " + "\r\n";
            queryString = queryString + "                                   GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + (!hasFoundCommoditiesByName && searchChassisEngineCode ? "AND (GoodsReceiptDetails.ChassisCode LIKE '%' + @SearchText + '%' OR GoodsReceiptDetails.EngineCode LIKE '%' + @SearchText + '%') " : "") + "\r\n";

            queryString = queryString + "                   END " + "\r\n";

            return queryString;
        }


        private void SearchServiceContracts()
        {
            string queryString = " @SearchText nvarchar(100) " + "\r\n"; 
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @ServiceContracts TABLE (ServiceContractID int, Reference nvarchar(10) NULL, CustomerID int NOT NULL, CommodityID int NOT NULL, PurchaseDate datetime NULL, LicensePlate nvarchar(60) NULL, ChassisCode nvarchar(60) NULL, EngineCode nvarchar(60) NULL, ColorCode nvarchar(60) NULL, AgentName nvarchar(100) NULL)" + "\r\n";

            queryString = queryString + "       IF (@SearchText <> '') " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               INSERT INTO @ServiceContracts SELECT ServiceContractID, Reference, CustomerID, CommodityID, PurchaseDate, LicensePlate, ChassisCode, EngineCode, ColorCode, AgentName FROM ServiceContracts WHERE LicensePlate LIKE '%' + @SearchText + '%' " + "\r\n";
            queryString = queryString + "               IF (@@ROWCOUNT <= 0) " + "\r\n"; 
            queryString = queryString + "                   INSERT INTO @ServiceContracts SELECT ServiceContractID, Reference, CustomerID, CommodityID, PurchaseDate, LicensePlate, ChassisCode, EngineCode, ColorCode, AgentName FROM ServiceContracts WHERE ChassisCode LIKE '%' + @SearchText + '%' OR EngineCode LIKE '%' + @SearchText + '%' " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       SELECT  ServiceContracts.ServiceContractID, ServiceContracts.Reference AS ServiceContractReference, ServiceContracts.CommodityID AS ServiceContractCommodityID, Commodities.Code AS ServiceContractCommodityCode, Commodities.Name AS ServiceContractCommodityName, ServiceContracts.PurchaseDate AS ServiceContractPurchaseDate, " + "\r\n";
            queryString = queryString + "               ServiceContracts.LicensePlate AS ServiceContractLicensePlate, ServiceContracts.ChassisCode AS ServiceContractChassisCode, ServiceContracts.EngineCode AS ServiceContractEngineCode, ServiceContracts.ColorCode AS ServiceContractColorCode, ServiceContracts.AgentName AS ServiceContractAgentName, " + "\r\n";
            queryString = queryString + "               ServiceContracts.CustomerID, Customers.Name AS CustomerName, Customers.Birthday AS CustomerBirthday, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";
            queryString = queryString + "       FROM    @ServiceContracts ServiceContracts INNER JOIN " + "\r\n";
            queryString = queryString + "               Commodities ON ServiceContracts.CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryString = queryString + "               Customers ON ServiceContracts.CustomerID = Customers.CustomerID INNER JOIN " + "\r\n";
            queryString = queryString + "               EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("SearchServiceContracts", queryString);
        }

        private void ServiceContractSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       UPDATE      SalesInvoiceDetails" + "\r\n";
            queryString = queryString + "       SET         QuantityContract = ROUND(SalesInvoiceDetails.QuantityContract + 1 * @SaveRelativeOption, 0)" + "\r\n";
            queryString = queryString + "       FROM        ServiceContracts INNER JOIN" + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails ON ServiceContracts.ServiceContractID = @EntityID AND ServiceContracts.SalesInvoiceDetailID = SalesInvoiceDetails.SalesInvoiceDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("ServiceContractSaveRelative", queryString);

        }

        private void ServiceContractPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Invoice Date: ' + CAST(SalesInvoiceDetails.EntryDate AS nvarchar) FROM ServiceContracts INNER JOIN SalesInvoiceDetails ON ServiceContracts.ServiceContractID = @EntityID AND ServiceContracts.SalesInvoiceDetailID = SalesInvoiceDetails.SalesInvoiceDetailID AND ServiceContracts.EntryDate < SalesInvoiceDetails.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = 'Over Quantity: ' + CAST(ROUND(Quantity - QuantityContract, 0) AS nvarchar) FROM SalesInvoiceDetails WHERE (ROUND(Quantity - QuantityContract, 0) < 0) ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("ServiceContractPostSaveValidate", queryArray);
        }

        private void ServicesContractDeletable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesInvoiceID FROM SalesInvoices WHERE ServiceContractID = @EntityID ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("ServicesContractDeletable", queryArray);
        }

        private void ServiceContractInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("ServiceContracts", "ServiceContractID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.ServiceContract));
            this.totalBikePortalsEntities.CreateTrigger("ServiceContractInitReference", simpleInitReference.CreateQuery());
        }
    }
}
