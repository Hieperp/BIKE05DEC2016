using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.CommonTasks
{
    public interface ICustomerCategoryRepository
    {
        IList<CustomerCategory> GetAllCustomerCategories();

        dynamic CustomerCategoriesForTreeView(int? id);
    }
}
