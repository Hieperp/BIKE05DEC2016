using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;

namespace MVCData.Repositories.CommonTasks
{
    public class CustomerTypeRepository : ICustomerTypeRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public CustomerTypeRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<CustomerType> GetAllCustomerTypes()
        {
            return this.totalBikePortalsEntities.CustomerTypes.ToList();
        }

        public dynamic CustomerTypesForTreeView(int? id)
        {
            var commodityTypes = from e in totalBikePortalsEntities.CustomerTypes
                                 where (id.HasValue ? e.AncestorID == id : e.AncestorID == null)
                                 select new
                                 {
                                     id = e.CustomerTypeID,
                                     Name = e.Name,
                                     hasChildren = e.CustomerTypes1.Any()
                                 };

            return commodityTypes;
        }
    }
      
}
