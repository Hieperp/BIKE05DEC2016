using System;
using System.Collections.Generic;
using System.Linq;

using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;

namespace MVCData.Repositories.CommonTasks
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public InventoryRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
            this.totalBikePortalsEntities.Database.CommandTimeout = 300;
        }

        public bool CheckOverStock(DateTime? checkedDate, string warehouseIDList, string commodityIDList)
        {
            List<OverStockItem> overStockItems = this.totalBikePortalsEntities.GetOverStockItems(checkedDate, warehouseIDList, commodityIDList).ToList();
            if (overStockItems.Count == 0)
                return true;
            else
            {
                string errorMessage = "Tính đến ngày: " + overStockItems[0].OverStockDate.ToShortDateString() + ", những mặt hàng sau không còn đủ số lượng tồn kho: " + "\r\n" + "\r\n";
                foreach (OverStockItem overStockItem in overStockItems)
                    errorMessage = errorMessage + "\t -----" + overStockItem.CommodityCode + "\t" + overStockItem.CommodityName + "\t" + "[" + overStockItem.Quantity.ToString("N0") + "]\t"  + "\r\n";

                throw new Exception(errorMessage);
            }

        }

    }
}
