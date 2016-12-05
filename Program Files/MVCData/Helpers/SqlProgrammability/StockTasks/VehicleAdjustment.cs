using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.StockTasks
{
    public class VehicleAdjustment
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public VehicleAdjustment(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }
        //CAI NAY MOI CHI CO COPY/ PASTE/ REPLACE THOI. CAN PHAI KIEM TRA LAI TAT CA
        public void RestoreProcedure()
        {
            this.GetVehicleAdjustmentIndexes();


            this.GetVehicleAdjustmentViewDetails();
            this.VehicleAdjustmentSaveRelative();
            this.VehicleAdjustmentPostSaveValidate();

            this.VehicleAdjustmentEditable();

            this.InventoryAdjustmentInitReference();
        }

        private void GetVehicleAdjustmentIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      InventoryAdjustments.InventoryAdjustmentID, CAST(InventoryAdjustments.EntryDate AS DATE) AS EntryDate, InventoryAdjustments.Reference, InventoryAdjustments.MemoNo, Locations.Code AS LocationCode, Suppliers.Name + ',    ' + Suppliers.AddressNo AS SupplierDescription, Commodities.Name AS CommodityName, InventoryAdjustmentDetails.GrossAmount " + "\r\n";
            queryString = queryString + "       FROM        InventoryAdjustments INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON InventoryAdjustments.InventoryAdjustmentTypeID = " + (int)GlobalEnums.InventoryAdjustmentTypeID.VehicleAdjustment + " AND InventoryAdjustments.EntryDate >= @FromDate AND InventoryAdjustments.EntryDate <= @ToDate AND InventoryAdjustments.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.VehicleAdjustment + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = InventoryAdjustments.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers Suppliers ON InventoryAdjustments.SupplierID = Suppliers.CustomerID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   InventoryAdjustmentDetails ON InventoryAdjustments.InventoryAdjustmentID = InventoryAdjustmentDetails.InventoryAdjustmentID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON InventoryAdjustmentDetails.CommodityID = Commodities.CommodityID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetVehicleAdjustmentIndexes", queryString);
        }



        private void GetVehicleAdjustmentViewDetails()
        {
            string queryString;

            queryString = " @InventoryAdjustmentID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      InventoryAdjustmentDetails.InventoryAdjustmentDetailID, InventoryAdjustmentDetails.InventoryAdjustmentID, InventoryAdjustmentDetails.GoodsReceiptDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, InventoryAdjustmentDetails.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue + InventoryAdjustmentDetails.Quantity, 0) AS QuantityAvailable, InventoryAdjustmentDetails.Quantity, InventoryAdjustmentDetails.UnitPrice, InventoryAdjustmentDetails.VATPercent, InventoryAdjustmentDetails.GrossPrice, InventoryAdjustmentDetails.Amount, InventoryAdjustmentDetails.VATAmount, InventoryAdjustmentDetails.GrossAmount, InventoryAdjustmentDetails.Remarks" + "\r\n";
            queryString = queryString + "       FROM        InventoryAdjustmentDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON InventoryAdjustmentDetails.InventoryAdjustmentID = @InventoryAdjustmentID AND InventoryAdjustmentDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON GoodsReceiptDetails.CommodityID = Commodities.CommodityID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetVehicleAdjustmentViewDetails", queryString);
        }



        private void VehicleAdjustmentSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsReceiptDetails" + "\r\n";
            queryString = queryString + "       SET         QuantityIssue = ROUND(GoodsReceiptDetails.QuantityIssue + InventoryAdjustmentDetails.Quantity * @SaveRelativeOption, 0)" + "\r\n";
            queryString = queryString + "       FROM       (SELECT GoodsReceiptDetailID, SUM(Quantity) AS Quantity FROM InventoryAdjustmentDetails WHERE InventoryAdjustmentID = @EntityID GROUP BY GoodsReceiptDetailID) InventoryAdjustmentDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON InventoryAdjustmentDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID ; " + "\r\n";

            //////queryString = queryString + "       THROW 60000, N'My Exception: Le Minh Hiep--throw', 1 " + "\r\n"; //RAISERROR (N'My Exception: Le Minh Hiep', 16, 1) 

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("VehicleAdjustmentSaveRelative", queryString);
        }

        private void VehicleAdjustmentPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Warehouse Date: ' + CAST(GoodsReceiptDetails.EntryDate AS nvarchar) FROM InventoryAdjustmentDetails INNER JOIN GoodsReceiptDetails ON InventoryAdjustmentDetails.InventoryAdjustmentID = @EntityID AND InventoryAdjustmentDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID AND InventoryAdjustmentDetails.EntryDate < GoodsReceiptDetails.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = 'Over Quantity: ' + CAST(ROUND(Quantity - QuantityIssue, 0) AS nvarchar) FROM GoodsReceiptDetails WHERE (ROUND(Quantity - QuantityIssue, 0) < 0) ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("VehicleAdjustmentPostSaveValidate", queryArray);
        }


        private void VehicleAdjustmentEditable()
        {
            string[] queryArray = new string[0];

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("VehicleAdjustmentEditable", queryArray);
        }


        private void InventoryAdjustmentInitReference()
        {
            InventoryAdjustmentInitReference simpleInitReference = new InventoryAdjustmentInitReference("InventoryAdjustments", "InventoryAdjustmentID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.InventoryAdjustment));
            this.totalBikePortalsEntities.CreateTrigger("InventoryAdjustmentInitReference", simpleInitReference.CreateQuery());
        }


    }
}
