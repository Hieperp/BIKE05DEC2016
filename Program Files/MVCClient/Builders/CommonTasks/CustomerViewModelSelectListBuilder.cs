using MVCClient.ViewModels.CommonTasks;
using MVCCore.Repositories.CommonTasks;

using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{
    public class CustomerViewModelSelectListBuilder : ICustomerViewModelSelectListBuilder
    {
        private readonly ICustomerCategoryRepository customerCategoryRepository;
        private readonly ICustomerCategorySelectListBuilder customerCategorySelectListBuilder;
        private readonly ICustomerTypeRepository customerTypeRepository;
        private readonly ICustomerTypeSelectListBuilder customerTypeSelectListBuilder;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public CustomerViewModelSelectListBuilder(ICustomerCategorySelectListBuilder customerCategorySelectListBuilder,
                                    ICustomerCategoryRepository customerCategoryRepository,
                                    ICustomerTypeRepository customerTypeRepository,
                                    ICustomerTypeSelectListBuilder customerTypeSelectListBuilder,
                                    IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.customerCategoryRepository = customerCategoryRepository;
            this.customerCategorySelectListBuilder = customerCategorySelectListBuilder;
            this.customerTypeRepository = customerTypeRepository;
            this.customerTypeSelectListBuilder = customerTypeSelectListBuilder;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public IEnumerable<SelectListItem> BuildSelectListItemsCustomers(IEnumerable<Customer> customers)
        {
            return customers.Where(w => w.IsCustomer == true).Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CustomerID.ToString()}).ToList();
        }

        public IEnumerable<SelectListItem> BuildSelectListItemsSuppliers(IEnumerable<Customer> suppliers)
        {
            return suppliers.Where(w => w.IsSupplier == true).Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CustomerID.ToString() }).ToList();
        }

        public void BuildSelectLists(CustomerViewModel customerViewModel)
        {
            customerViewModel.CustomerCategoryDropDown = customerCategorySelectListBuilder.BuildSelectListItemsForCustomerCategories(customerCategoryRepository.GetAllCustomerCategories());
            customerViewModel.CustomerTypeDropDown = customerTypeSelectListBuilder.BuildSelectListItemsForCustomerCategories(customerTypeRepository.GetAllCustomerTypes());
        }
        
    }
}