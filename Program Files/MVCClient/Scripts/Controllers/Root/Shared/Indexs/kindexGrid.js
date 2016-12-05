define([], (function () {

    var definedExemplar = function (kenGridName) {
        this._kenGrid = $("#" + kenGridName).data("kendoGrid");
        //this.pageIndex = 0;
    };


    definedExemplar.prototype.setSelectedRow = function (idName, idValue) {
        if (idValue <= 0) return;
        var that = this;
        $.each(this._kenGrid.dataSource.data(), function (idx, elem) {
            if (elem[idName] == idValue) {
                
                $('[data-uid=' + elem.uid + ']').addClass('k-state-selected');

                var pageIndex = Math.floor(idx / that._kenGrid.dataSource.pageSize() + 1);

                //if (pageIndex >= 0) {
                //    that._kenGrid.dataSource.fetch(function () {
                //        that._kenGrid.dataSource.page(pageIndex);
                //    });
                //}
                return false;
            }
        });
    };

    definedExemplar.prototype.showName = function (idName, idValue) { alert(this._kenGrid); }

    return definedExemplar;
}));