using MVCModel.Models;
using MVCDTO.CommonTasks;
using MVCCore.Repositories.CommonTasks;
using MVCCore.Services.CommonTasks;


namespace MVCService.CommonTasks
{
    public class CustomerService : GenericService<Customer, CustomerDTO, CustomerPrimitiveDTO>, ICustomerService
    {
        public CustomerService(ICustomerRepository customerRepository)
            : base(customerRepository)
        {
        }

    }
}
