define([], (function () {

    $(document).ready(function () {
        if (!requireConfig.pageOptions.Editable)
            $("input").each(function (index, element) {
                var widgetObject = kendo.widgetInstance($(element), kendo.ui);
                if (typeof widgetObject != 'undefined') widgetObject.readonly(true);
            });

    });

}));
