using System.Collections.Generic;

using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;

namespace MVCCore.Repositories.CommonTasks
{   
    public interface IEntireTerritoryRepository : IGenericRepository<EntireTerritory>
    {        
        IList<EntireTerritory> SearchEntireTerritoriesByName(string name);

        IList<EntireTerritory> GetAllEntireTerritories();
    }
}
