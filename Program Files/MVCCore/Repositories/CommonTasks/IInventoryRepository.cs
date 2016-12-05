using System;

namespace MVCCore.Repositories.CommonTasks
{
    public interface IInventoryRepository
    {
        bool CheckOverStock(DateTime? checkedDate, string warehouseIDList, string commodityIDList);
    }
}
