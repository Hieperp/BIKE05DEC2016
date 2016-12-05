using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.CommonTasks
{
    public interface ICommodityCategoryRepository
    {
        IList<CommodityCategory> GetAllCommodityCategories();

        dynamic CommodityCategoriesForTreeView(int? id);
    }
}
