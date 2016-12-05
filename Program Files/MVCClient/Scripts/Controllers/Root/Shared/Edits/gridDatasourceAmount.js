define(["superBase", "gridDatasourceQuantity"], (function (superBase, gridDatasourceQuantity) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasourceQuantity);






    definedExemplar.prototype._removeTotalToModelProperty = function () {
        this._updateTotalToModelProperty("TotalAmount", "Amount", "sum", false);
        this._updateTotalToModelProperty("TotalVATAmount", "VATAmount", "sum", false);
        this._updateTotalToModelProperty("TotalGrossAmount", "GrossAmount", "sum", false);

        definedExemplar._super._removeTotalToModelProperty.call(this);
    }








    definedExemplar.prototype._changeQuantity = function (dataRow) {
        this._updateRowAmount(dataRow);
        this._updateRowGrossAmount(dataRow);

        definedExemplar._super._changeQuantity.call(this, dataRow);
    }

    definedExemplar.prototype._changeUnitPrice = function (dataRow) {
        this._updateRowGrossPrice(dataRow);
        this._updateRowAmount(dataRow);
    }

    definedExemplar.prototype._changeVATPercent = function (dataRow) {
        this._updateRowGrossPrice(dataRow);
    }

    definedExemplar.prototype._changeGrossPrice = function (dataRow) {
        this._updateRowUnitPrice(dataRow);
        this._updateRowGrossAmount(dataRow);
    }

    definedExemplar.prototype._changeAmount = function (dataRow) {
        this._updateRowVATAmount(dataRow);

        this._updateTotalToModelProperty("TotalAmount", "Amount", "sum");
    }

    definedExemplar.prototype._changeVATAmount = function (dataRow) {
        this._updateTotalToModelProperty("TotalVATAmount", "VATAmount", "sum");
    }

    definedExemplar.prototype._changeGrossAmount = function (dataRow) {
        this._updateRowVATAmount(dataRow);

        this._updateTotalToModelProperty("TotalGrossAmount", "GrossAmount", "sum");
    }





    definedExemplar.prototype._updateRowUnitPrice = function (dataRow) {
        var newUnitPrice = dataRow.GrossPrice * 100 / (100 + dataRow.VATPercent);
        if (dataRow.UnitPrice - newUnitPrice > 0.8 || newUnitPrice - dataRow.UnitPrice > 0.8)
            dataRow.set("UnitPrice", Math.round(newUnitPrice, requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowGrossPrice = function (dataRow) {
        var newGrossPrice = dataRow.UnitPrice * (1 + dataRow.VATPercent / 100);
        if (dataRow.GrossPrice - newGrossPrice > 0.8 || newGrossPrice - dataRow.GrossPrice > 0.8)
            dataRow.set("GrossPrice", Math.round(newGrossPrice, requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowAmount = function (dataRow) {
        dataRow.set("Amount", Math.round(dataRow.Quantity * dataRow.UnitPrice, requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowVATAmount = function (dataRow) {
        dataRow.set("VATAmount", Math.round(dataRow.GrossAmount - dataRow.Amount, requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowGrossAmount = function (dataRow) {
        dataRow.set("GrossAmount", Math.round(dataRow.Quantity * dataRow.GrossPrice, requireConfig.websiteOptions.rndAmount));
    }


    return definedExemplar;
}));