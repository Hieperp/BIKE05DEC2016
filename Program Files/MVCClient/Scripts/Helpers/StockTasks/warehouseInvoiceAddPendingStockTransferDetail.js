function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(warehouseInvoiceGridDataSource, pendingStockTransferDetailGridDataSource) {
    if (warehouseInvoiceGridDataSource != undefined && pendingStockTransferDetailGridDataSource != undefined) {
        var pendingStockTransferDetailGridDataItems = pendingStockTransferDetailGridDataSource.view();
        var warehouseInvoiceJSON = warehouseInvoiceGridDataSource.data().toJSON();
        for (var i = 0; i < pendingStockTransferDetailGridDataItems.length; i++) {
            if (pendingStockTransferDetailGridDataItems[i].IsSelected === true)
                _setParentInput(warehouseInvoiceJSON, pendingStockTransferDetailGridDataItems[i]);
        }

        warehouseInvoiceJSON.push(new Object()); //Add a temporary empty row

        warehouseInvoiceGridDataSource.data(warehouseInvoiceJSON);

        //////var dataRowTest = warehouseInvoiceGridDataSource.add({}); //To calculate total

        var rawData = warehouseInvoiceGridDataSource.data()
        warehouseInvoiceGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

        //warehouseInvoiceGridDataSource.trigger("change");

        cancelButton_Click();
    }


    //http://www.telerik.com/forums/adding-multiple-rows-performance
    //By design the dataSource does not provide an opportunity for inserting multiple records via one operation. The performance is low, because each time when you add row through the addRow method, the DataSource throws its change event which forces the Grid to refresh and re-paint the content.
    //To avoid the problem you may try to modify the data of the DataSource manually.
    //var grid = $("#grid").data("kendoGrid");
    //var data = gr.dataSource.data().toJSON(); //the data of the DataSource

    ////change the data array
    ////any changes in the data array will not automatically reflect in the Grid

    //grid.dataSource.data(data); //set changed data as data of the Grid


    function _setParentInput(warehouseInvoiceJSON, transferOrderGridDataItem) {

        //var dataRow = warehouseInvoiceJSON.add({});

        var dataRow = new Object();

        dataRow.WarehouseInvoiceDetailID = 0;
        dataRow.WarehouseInvoiceID = window.parent.$("#WarehouseInvoiceID").val();
        dataRow.EntryDate = null;
        dataRow.LocationID = null;
        dataRow.Remarks = null;

        dataRow.StockTransferDetailID = transferOrderGridDataItem.StockTransferDetailID;

        dataRow.WarehouseID = transferOrderGridDataItem.WarehouseID;
        dataRow.CommodityID = transferOrderGridDataItem.CommodityID;
        dataRow.CommodityName = transferOrderGridDataItem.CommodityName;
        dataRow.CommodityCode = transferOrderGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = transferOrderGridDataItem.CommodityTypeID;



        dataRow.Quantity = transferOrderGridDataItem.Quantity;
        dataRow.ListedPrice = transferOrderGridDataItem.ListedPrice;
        dataRow.DiscountPercent = transferOrderGridDataItem.DiscountPercent;
        dataRow.UnitPrice = transferOrderGridDataItem.UnitPrice;
        dataRow.VATPercent = transferOrderGridDataItem.VATPercent;
        dataRow.GrossPrice = transferOrderGridDataItem.GrossPrice;
        dataRow.Amount = transferOrderGridDataItem.Amount;
        dataRow.VATAmount = transferOrderGridDataItem.VATAmount;
        dataRow.GrossAmount = transferOrderGridDataItem.GrossAmount;

        dataRow.ChassisCode = transferOrderGridDataItem.ChassisCode;
        dataRow.EngineCode = transferOrderGridDataItem.EngineCode;
        dataRow.ColorCode = transferOrderGridDataItem.ColorCode;

        warehouseInvoiceJSON.push(dataRow);
    }
}

