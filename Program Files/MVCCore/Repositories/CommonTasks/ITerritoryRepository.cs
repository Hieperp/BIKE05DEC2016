using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.CommonTasks
{
    public interface ITerritoryRepository
    {
        IList<Territory> GetAllTerritories();

        dynamic TerritoriesForTreeView(int? id);
    }
}
