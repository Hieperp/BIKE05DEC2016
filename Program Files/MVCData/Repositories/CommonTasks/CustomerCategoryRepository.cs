using System.Linq;
using System.Collections.Generic;


using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;


namespace MVCData.Repositories.CommonTasks
{

    public class CustomerCategoryRepository : ICustomerCategoryRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public CustomerCategoryRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<CustomerCategory> GetAllCustomerCategories()
        {
            return this.totalBikePortalsEntities.CustomerCategories.ToList();
        }

        public dynamic CustomerCategoriesForTreeView(int? id)
        {
            var commodityCategories = from e in totalBikePortalsEntities.CustomerCategories
                            where (id.HasValue ? e.AncestorID == id : e.AncestorID == null)
                            select new
                            {
                                id = e.CustomerCategoryID,
                                Name = e.Name,
                                hasChildren = e.CustomerCategories1.Any()
                            };

            return commodityCategories;
        }
    }
}
