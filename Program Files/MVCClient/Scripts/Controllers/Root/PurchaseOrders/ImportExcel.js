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

        var xlsxWorkbookInstance = new xlsxWorkbook(["CommodityCode", "CommodityName", "CommodityOriginalName", "Quantity", "UnitPrice", "ChassisCode", "EngineCode", "ColorCode"]);
        if (xlsxWorkbookInstance.checkValidColumn(excelRowCollection.ImportSheet)) {

            var gridDataSource = $("#kendoGridDetails").data("kendoGrid").dataSource;

            for (i = 0; i < excelRowCollection.ImportSheet.length; i++) {

                var dataRow = gridDataSource.add({});
                var excelRow = excelRowCollection.ImportSheet[i];

                dataRow.set("Quantity", Math.round(excelRow["Quantity"], requireConfig.websiteOptions.rndAmount));
                dataRow.set("UnitPrice", Math.round(excelRow["UnitPrice"], requireConfig.websiteOptions.rndAmount));

                dataRow.set("ChassisCode", excelRow["ChassisCode"] === undefined? "" : excelRow["ChassisCode"].trim());
                dataRow.set("EngineCode", excelRow["EngineCode"] === undefined ? "" : excelRow["EngineCode"].trim());
                dataRow.set("ColorCode", excelRow["ColorCode"] === undefined ? "" : excelRow["ColorCode"].trim());

                _getCommoditiesByCode(dataRow, excelRow);
            }
        }
        else {
            alert("Lỗi import dữ liệu. Vui lòng kiểm tra file excel cẩn thận trước khi thử import lại");
        }



        function _getCommoditiesByCode(dataRow, excelRow) {
            return $.ajax({
                url: window.urlCommoditiesApi,
                data: JSON.stringify({ "code": excelRow["CommodityCode"], "name": excelRow["CommodityName"], "originalName": excelRow["CommodityOriginalName"], "commodityTypeID": (excelRow["ChassisCode"] != undefined && excelRow["EngineCode"] != undefined && excelRow["ColorCode"] != undefined && excelRow["ChassisCode"].trim().length > 0 && excelRow["EngineCode"].trim().length > 0 && excelRow["ColorCode"].trim().length > 0 ? 1 : 2), "commodityCategoryID": 4 }),
                type: 'POST',
                contentType: 'application/json;',
                dataType: 'json',
                success: function (result) {
                    if (result.CommodityID > 0) {
                        dataRow.set("CommodityID", result.CommodityID);
                        dataRow.set("CommodityCode", result.Code);
                        dataRow.set("CommodityName", result.Name);
                        dataRow.set("CommodityTypeID", result.CommodityTypeID);
                        dataRow.set("VATPercent", Math.round(result.VATPercent, requireConfig.websiteOptions.rndAmount));
                    }
                    else
                        dataRow.set("CommodityCode", result.Code);
                },
                error: function (jqXHR, textStatus) {
                    dataRow.set("CommodityCode", textStatus);
                }
            });
        }



    }




});