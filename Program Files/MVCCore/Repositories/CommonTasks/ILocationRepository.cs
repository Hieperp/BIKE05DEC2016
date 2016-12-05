using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.CommonTasks
{
    public interface ILocationRepository
    {
        IList<Location> SearchLocationsByName(int? filterLocationID, string searchText);
    }
}
