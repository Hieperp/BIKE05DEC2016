require(["xlsxNmvn", "xlsxWorkbook"], function (xlsxNmvn, xlsxWorkbook) {

    $(document).ready(function () {
        var xlf = document.getElementById('xlf');

        if (xlf.addEventListener) {
            xlf.addEventListener('change', handleFile, false);
        }
    });




    process_wb = function (wb) {

        var jsonWorkBook = JSON.stringify(to_json(wb), 2, 2); //jsonWorkBook = to_formulae(wb); //jsonWorkBook = to_csv(wb);
        var excelRowCollection = JSON.parse(jsonWorkBook);

        var xlsxWorkbookInstance = new xlsxWorkbook(["CommodityCode", "Quantity"]);
        if (xlsxWorkbookInstance.checkValidColumn(excelRowCollection.ImportSheet)) {

            var gridDataSource = $("#kendoGridDetails").data("kendoGrid").dataSource;

            for (i = 0; i < excelRowCollection.ImportSheet.length; i++) {

                var dataRow = gridDataSource.add({});
                var excelRow = excelRowCollection.ImportSheet[i];

                dataRow.set("Quantity", Math.round(excelRow["Quantity"], requireConfig.websiteOptions.rndQuantity));
                dataRow.set("Remarks", excelRow["Remarks"]);

                _getCommoditiesByCode(dataRow, excelRow);
            }
        }
        else {
            alert("Lỗi import dữ liệu. Vui lòng kiểm tra file excel cẩn thận trước khi thử import lại");
        }



        function _getCommoditiesByCode(dataRow, excelRow) {
            return $.ajax({
                url: window.urlCommoditiesApi,
                data: JSON.stringify({ "locationID": ($("#SourceLocationID").val() != undefined ? $("#SourceLocationID").val() : requireConfig.pageOptions.LocationID), "entryDate": $("#EntryDate").data("kendoDateTimePicker").value().toUTCString(), "searchText": excelRow["CommodityCode"], "wholeMatch": false }),
                type: 'POST',
                contentType: 'application/json;',
                dataType: 'json',
                success: function (result) {
                    if (result.length > 0) {
                        if (result[0].CommodityID > 0) {
                            dataRow.set("CommodityID", result[0].CommodityID);
                            dataRow.set("CommodityCode", result[0].CommodityCode);
                            dataRow.set("CommodityName", result[0].CommodityName);
                            dataRow.set("CommodityTypeID", result[0].CommodityTypeID);

                            dataRow.set("WarehouseID", result[0].WarehouseID);
                            dataRow.set("WarehouseCode", result[0].WarehouseCode);

                            dataRow.set("QuantityAvailable", Math.round(result[0].QuantityAvailable, requireConfig.websiteOptions.rndQuantity));
                        }
                        else
                            dataRow.set("CommodityCode", result[0].CommodityCode);
                    }
                },
                error: function (jqXHR, textStatus) {
                    dataRow.set("CommodityCode", textStatus);
                }
            });
        }



    }




});