using MVCModel.Models;
using MVCDTO.CommonTasks;

namespace MVCCore.Services.CommonTasks
{
    public interface ICustomerService : IGenericService<Customer, CustomerDTO, CustomerPrimitiveDTO>
    {
    }
}
