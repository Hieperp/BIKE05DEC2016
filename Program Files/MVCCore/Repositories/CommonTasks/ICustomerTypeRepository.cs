using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.CommonTasks
{
    public interface ICustomerTypeRepository
    {
        IList<CustomerType> GetAllCustomerTypes();

        dynamic CustomerTypesForTreeView(int? id);
    }
}
