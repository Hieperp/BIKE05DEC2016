﻿define(["superBase", "gridDatasourceAmount"], (function (superBase, gridDatasourceAmount) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasourceAmount);






    definedExemplar.prototype._removeTotalToModelProperty = function () {
        this._updateTotalToModelProperty("AverageDiscountPercent", "DiscountPercent", "average", false);

        definedExemplar._super._removeTotalToModelProperty.call(this);
    }







    definedExemplar.prototype._changeCommodityID = function (dataRow) {
        if (dataRow.ListedPrice != undefined) {
            dataRow.set("ListedPrice", 0);
        }
    }






    definedExemplar.prototype._changeListedPrice = function (dataRow) {
        this._updateRowUnitPriceByListedPrice(dataRow);
    }

    definedExemplar.prototype._changeDiscountPercent = function (dataRow) {
        this._updateRowUnitPriceByListedPrice(dataRow);

        this._updateTotalToModelProperty("AverageDiscountPercent", "DiscountPercent", "average");
    }

    definedExemplar.prototype._changeUnitPrice = function (dataRow) {
        this._updateRowDiscountPercent(dataRow);
        this._updateRowGrossPrice(dataRow);
        this._updateRowAmount(dataRow);
    }

    definedExemplar.prototype._changeGrossPrice = function (dataRow) {
        this._updateRowUnitPrice(dataRow);
        this._updateRowGrossAmount(dataRow);
    }




    definedExemplar.prototype._updateRowDiscountPercent = function (dataRow) {
        var newDiscountPercent = dataRow.ListedPrice === 0 ? 0 : (dataRow.ListedPrice - dataRow.UnitPrice) * 100 / dataRow.ListedPrice;
        if (dataRow.DiscountPercent - newDiscountPercent > 0.08 || newDiscountPercent - dataRow.DiscountPercent > 0.08)
            dataRow.set("DiscountPercent", this._round(newDiscountPercent, requireConfig.websiteOptions.rndDiscountPercent));
    }

    definedExemplar.prototype._updateRowUnitPriceByListedPrice = function (dataRow) {
        var newUnitPrice = dataRow.ListedPrice * (1 - dataRow.DiscountPercent / 100);
        if (dataRow.UnitPrice - newUnitPrice > 0.8 || newUnitPrice - dataRow.UnitPrice > 0.8)
            dataRow.set("UnitPrice", Math.round(newUnitPrice, requireConfig.websiteOptions.rndAmount));
    }


    return definedExemplar;
}));