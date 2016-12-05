define(["gridWidgetQuantityRemains"], (function (gridWidgetQuantityRemains) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").bind("change", function (e) {
            var gridWidgetQuantityRemainsInstance = new gridWidgetQuantityRemains();
            gridWidgetQuantityRemainsInstance.copyEditingToSelectedCell(e);
        });

    });
}));
