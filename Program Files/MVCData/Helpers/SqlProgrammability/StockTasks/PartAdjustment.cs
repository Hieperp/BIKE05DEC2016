using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;


namespace MVCData.Helpers.SqlProgrammability.StockTasks
{
    public class PartAdjustment
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public PartAdjustment(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPartAdjustmentIndexes();

            this.GetPartAdjustmentViewDetails();
            this.PartAdjustmentSaveRelative();
            this.PartAdjustmentPostSaveValidate();

            this.PartAdjustmentEditable();
        }


        private void GetPartAdjustmentIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      InventoryAdjustments.InventoryAdjustmentID, CAST(InventoryAdjustments.EntryDate AS DATE) AS EntryDate, InventoryAdjustments.Reference, Locations.Code AS LocationCode, Suppliers.Name + ',    ' + Suppliers.AddressNo AS SupplierDescription, InventoryAdjustments.TotalGrossAmount " + "\r\n";
            queryString = queryString + "       FROM        InventoryAdjustments INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON InventoryAdjustments.InventoryAdjustmentTypeID = " + (int)GlobalEnums.InventoryAdjustmentTypeID.PartAdjustment + " AND InventoryAdjustments.EntryDate >= @FromDate AND InventoryAdjustments.EntryDate <= @ToDate AND InventoryAdjustments.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.PartAdjustment + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = InventoryAdjustments.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers Suppliers ON InventoryAdjustments.SupplierID = Suppliers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPartAdjustmentIndexes", queryString);
        }



        #region X


        private void GetPartAdjustmentViewDetails()
        {
            string queryString;
            SqlProgrammability.StockTasks.Inventories inventories = new StockTasks.Inventories(this.totalBikePortalsEntities);

            queryString = " @InventoryAdjustmentID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @LocationID varchar(35)      DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";
            queryString = queryString + "       SELECT      @EntryDate = EntryDate, @LocationID = LocationID FROM InventoryAdjustments WHERE InventoryAdjustmentID = @InventoryAdjustmentID " + "\r\n";
            queryString = queryString + "       IF          @EntryDate IS NULL          SET @EntryDate = CONVERT(Datetime, '31/12/2000', 103)" + "\r\n";
            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID as varchar)  FROM Warehouses WHERE LocationID = @LocationID FOR XML PATH('')) ,1,1,'') " + "\r\n";//The best way is get the @WarehouseIDList from table InventoryAdjustmentDetails, but we don't want the stored procedure read from InventoryAdjustmentDetails to save the resource
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID as varchar)  FROM InventoryAdjustmentDetails WHERE InventoryAdjustmentID = @InventoryAdjustmentID FOR XML PATH('')) ,1,1,'') " + "\r\n";

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";

            queryString = queryString + "       SELECT      InventoryAdjustmentDetails.InventoryAdjustmentDetailID, InventoryAdjustmentDetails.InventoryAdjustmentID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, InventoryAdjustmentDetails.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, " + "\r\n";
            queryString = queryString + "                   ROUND(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) - InventoryAdjustmentDetails.Quantity, 0) AS QuantityAvailable, InventoryAdjustmentDetails.Quantity, InventoryAdjustmentDetails.UnitPrice, InventoryAdjustmentDetails.VATPercent, InventoryAdjustmentDetails.GrossPrice, InventoryAdjustmentDetails.Amount, InventoryAdjustmentDetails.VATAmount, InventoryAdjustmentDetails.GrossAmount, InventoryAdjustmentDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        InventoryAdjustmentDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON InventoryAdjustmentDetails.InventoryAdjustmentID = @InventoryAdjustmentID AND InventoryAdjustmentDetails.CommodityID = Commodities.CommodityID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON InventoryAdjustmentDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN" + "\r\n";
            queryString = queryString + "                  (SELECT WarehouseID, CommodityID, SUM(QuantityEndREC) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON InventoryAdjustmentDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND InventoryAdjustmentDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPartAdjustmentViewDetails", queryString);
        }

        private void PartAdjustmentSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       EXEC        UpdateWarehouseBalance @SaveRelativeOption, 0, 0, 0, @EntityID ";

            this.totalBikePortalsEntities.CreateStoredProcedure("PartAdjustmentSaveRelative", queryString);
        }

        private void PartAdjustmentPostSaveValidate()
        {
            string[] queryArray = new string[0];

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("PartAdjustmentPostSaveValidate", queryArray);
        }







        private void PartAdjustmentEditable()
        {
            string[] queryArray = new string[0];

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("PartAdjustmentEditable", queryArray);
        }



        #endregion
    }
}
