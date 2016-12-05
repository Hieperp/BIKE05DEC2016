//BEGIN Handle Grid Events


//Important: This function run ok only when one cell in edit mode
//This avoid redrawing the Grid rows when use the 'dataItem.set()' method
//Note: When use the 'dataItem.set()' method: right after call 'set' the grid is repainted and the rows are receiving different uid attributes i.e. the previously selected row does not exist anymore. This will raise error when handle for the next selected row.
function kendoGridDetails_Change(e) {

    var editingCellRowIndex = $(".k-edit-cell").closest("tr").index(); //Check current editing row index

    if (editingCellRowIndex >= 0) {//Current editing cell found

        var editingCellColIndex = $(".k-edit-cell").first().index(); //Check current editing col index
        var selectedRows = this.select();

        if (editingCellColIndex > 0 && selectedRows.length > 1) { //Current editing cell found && user drag to select more than 1 row.

            //Get the current editing field name
            var gridHeader = this.thead;
            var thGridHeader = $(gridHeader).find("th").eq(editingCellColIndex);
            var editingCellFieldName = $(thGridHeader).data("field");

            //Get the current editing cell data
            var gridData = this.dataSource.data();
            var editingDataRow = gridData[editingCellRowIndex];
            var editingDataCell = editingDataRow.get(editingCellFieldName);

            switch(editingCellFieldName)
            {
                case "WarehouseName":
                    var warehouseID = editingDataRow.get("WarehouseID");

                    for (var i = 0; i < selectedRows.length; i++) {

                        var dataItem = this.dataItem(selectedRows[i]);
                        dataItem.set("WarehouseID", warehouseID);
                        dataItem.set("WarehouseName", editingDataCell);//Set all selected rows the same value of the current editing cell
                    }
                    break;
                case "Quantity":
                    var quantityRemains = editingDataRow.get("QuantityRemains");
                    for (var i = 0; i < selectedRows.length; i++) {

                        var dataItem = this.dataItem(selectedRows[i]);                       
                        dataItem.set("Quantity", quantityRemains);//Set all selected rows the same value of the current editing cell
                    }
                    break;
                default:
                    for (var i = 0; i < selectedRows.length; i++) {

                        var dataItem = this.dataItem(selectedRows[i]);
                        dataItem.set(editingCellFieldName, editingDataCell)//Set all selected rows the same value of the current editing cell
                    }
                    break;
            }          
         
            e.sender.refresh();
        }
    }
}



//END Handle Grid Events