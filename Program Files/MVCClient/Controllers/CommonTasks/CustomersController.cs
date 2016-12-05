using System.Web.Mvc;
using System.Net;
using AutoMapper;

using MVCModel.Models;

using MVCCore.Services.CommonTasks;
using MVCDTO.CommonTasks;
using MVCClient.ViewModels.CommonTasks;
using MVCClient.Builders.CommonTasks;


namespace MVCClient.Controllers.SalesTasks
{
    public class CustomersController : GenericSimpleController<Customer, CustomerDTO, CustomerPrimitiveDTO, CustomerViewModel>
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService, ICustomerViewModelSelectListBuilder customerViewModelSelectListBuilder)
            : base(customerService, customerViewModelSelectListBuilder)
        {
            this.customerService = customerService;
        }

        public ActionResult CreatePopup()
        {
            CustomerViewModel customerViewModel = new CustomerViewModel();
            customerViewModel.IsCustomer = true;
            return View(this.TailorViewModel(customerViewModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePopup(CustomerViewModel customerViewModel)
        {
            if (this.Save(customerViewModel))
                return RedirectToAction("SuccessPopup", new { id = customerViewModel.CustomerID });
            else
                return View(this.TailorViewModel(customerViewModel));
        }

        public ActionResult SuccessPopup(int? id)
        {
            Customer customer;
            if (id == null || (customer = this.customerService.GetByID((int)id)) == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(Mapper.Map<CustomerViewModel>(customer));
        }


    }
}

