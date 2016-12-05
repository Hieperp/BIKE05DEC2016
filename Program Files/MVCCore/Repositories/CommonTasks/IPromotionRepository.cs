using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.CommonTasks
{
    public interface IPromotionRepository
    {
        IList<Promotion> SearchPromotions(int? locationID, string searchText);
    }
}
