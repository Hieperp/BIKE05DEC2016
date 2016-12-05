define([], (function () {

    var definedExemplar = function () {
        
    };


    //Important: This function run ok only when one cell in edit mode
    //This avoid redrawing the Grid rows when use the 'dataItem.set()' method
    //Important Note: When use the 'dataItem.set()' method: right after call 'set' the grid is repainted and the rows are receiving different uid attributes i.e. the previously selected row does not exist anymore. This will raise error when handle for the next selected row.
    definedExemplar.prototype.copyEditingToSelectedCell = function (e) {
        var editingCellRowIndex = $(".k-edit-cell").closest("tr").index(); //Check current editing row index

        if (editingCellRowIndex >= 0) {//Current editing cell found

            var editingCellColIndex = $(".k-edit-cell").first().index(); //Check current editing col index
            var selectedRows = e.sender.select();

            if (editingCellColIndex > 0 && selectedRows.length > 1) { //Current editing cell found && user drag to select more than 1 row.

                //Get the current editing field name
                var gridHeader = e.sender.thead;
                var thGridHeader = $(gridHeader).find("th").eq(editingCellColIndex);
                var editingCellFieldName = $(thGridHeader).data("field");

                //Get the current editing cell data
                var gridData = e.sender.dataSource.data();
                var editingDataRow = gridData[editingCellRowIndex];

                for (var i = 0; i < selectedRows.length; i++) {

                    var dataItem = e.sender.dataItem(selectedRows[i]);
                    this._setCellValue(dataItem, editingDataRow, editingCellFieldName);

                }

                e.sender.refresh();
            }
        }
    };


    definedExemplar.prototype._setCellValue = function (dataItem, editingDataRow, editingCellFieldName) {
        dataItem.set(editingCellFieldName, editingDataRow.get(editingCellFieldName));//Set all selected rows the same value of the current editing cell
    }


    return definedExemplar;

}));