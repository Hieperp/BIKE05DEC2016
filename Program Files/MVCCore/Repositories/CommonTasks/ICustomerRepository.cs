using System.Collections.Generic;

using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;

namespace MVCCore.Repositories.CommonTasks
{   
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        IList<Customer> SearchSuppliersByName(string name);
        IList<Customer> SearchCustomersByName(string name);
        IList<Customer> SearchCustomersByIndex(int customerCategoryID, int customerTypeID, int territoryID);

        IList<Customer> GetAllCustomers();
        IList<Customer> GetAllSuppliers();     
    }
}
