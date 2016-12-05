using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.CommonTasks
{
    public interface IWarehouseRepository
    {
        IList<Warehouse> GetAllWarehouses();
        IList<Warehouse> SearchWarehousesByName(int? locationID, string searchText);
    }
}
