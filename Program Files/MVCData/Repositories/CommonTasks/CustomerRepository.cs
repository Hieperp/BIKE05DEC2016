using System.Collections.Generic;
using System.Linq;

using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;

namespace MVCData.Repositories.CommonTasks
{   
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities)
        {
        }

        public IList<Customer> SearchSuppliersByName(string name)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> suppliers = this.TotalBikePortalsEntities.Customers.Include("EntireTerritory").Where(w => w.IsSupplier && (w.Name.Contains(name) || w.VATCode.Contains(name))).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return suppliers;
        }

        public IList<Customer> SearchCustomersByName(string name)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = this.TotalBikePortalsEntities.Customers.Include("EntireTerritory").Where(w => w.IsCustomer && (w.Name.Contains(name) || w.VATCode.Contains(name))).OrderByDescending(or => or.CustomerID).Take(20).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return customers;
        }

        public IList<Customer> SearchCustomersByIndex(int customerCategoryID, int customerTypeID, int territoryID)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = this.TotalBikePortalsEntities.Customers.Where(w => w.CustomerCategoryID == customerCategoryID || w.CustomerTypeID == customerTypeID || w.TerritoryID == territoryID).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return customers;
        }

        public IList<Customer> GetAllCustomers()
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = this.TotalBikePortalsEntities.Customers.Where(w => w.IsCustomer).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return customers;
        }

        public IList<Customer> GetAllSuppliers()
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> suppliers = this.TotalBikePortalsEntities.Customers.Where(w => w.IsSupplier).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return suppliers;
        }
    }
}
