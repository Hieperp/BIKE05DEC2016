using System;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using System.Web;


using AutoMapper;
using RequireJsNet;

using MVCBase.Enums;
using MVCModel;
using MVCDTO;
using MVCCore.Services;

using MVCClient.Builders;
using MVCClient.ViewModels.Helpers;
using MVCClient.ViewModels.SalesTasks;


namespace MVCClient.Controllers
{
    [GenericSimpleAuthorizeAttribute]
    public abstract class GenericSimpleController<TEntity, TDto, TPrimitiveDto, TSimpleViewModel> : BaseController

        where TEntity : class, IPrimitiveEntity, IBaseEntity, new()
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TSimpleViewModel : TDto, ISimpleViewModel, new() //Note: constraints [TSimpleViewModel : TDto] and also [TViewDetailViewModel : TDto  -> in GenericViewDetailController]: is required for this.genericService.Editable(TDto) only!!! If there is any reason need to remove this constraints, just consider for this.genericService.Editable(TDto) only [should change this.genericService.Editable(TDto) only if needed -- means after remove this constraints]
    {
        protected readonly IGenericService<TEntity, TDto, TPrimitiveDto> GenericService;
        private readonly IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder;

        private bool isSimpleCreate;
        private bool isCreateWizard;




        public GenericSimpleController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder)
            : this(genericService, viewModelSelectListBuilder, false, true)
        {
        }

        public GenericSimpleController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder, bool isCreateWizard)
            : this(genericService, viewModelSelectListBuilder, isCreateWizard, false)
        {
        }

        public GenericSimpleController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder, bool isCreateWizard, bool isSimpleCreate)
            : base(genericService)
        {
            this.GenericService = genericService;
            this.viewModelSelectListBuilder = viewModelSelectListBuilder;

            this.isCreateWizard = isCreateWizard;
            this.isSimpleCreate = isSimpleCreate;
        }



        //[AccessLevelAuthorize(GlobalEnums.AccessLevel.Readable)]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Index(int? id)
        {
            ViewBag.SelectedEntityID = id == null ? -1 : (int)id;
            return View();
        }








        /// <summary>
        /// Create NEW from an empty ViewModel object
        /// </summary>
        /// <returns></returns>
        [AccessLevelAuthorize]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Create()
        {
            if (!this.isSimpleCreate) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            return View(this.TailorViewModel(new TSimpleViewModel())); //Need to call new TSimpleViewModel() to ensure construct TSimpleViewModel object using Constructor!
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Create(TSimpleViewModel simpleViewModel)
        {
            if (!this.isSimpleCreate) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            if (this.Save(simpleViewModel))
                return RedirectToAction(simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Save ? "Edit" : simpleViewModel.PrintAfterClosedSubmit ? "Print" : "Index", new { id = simpleViewModel.GetID() });
            else
            {
                return View(this.TailorViewModel(simpleViewModel));
            }
        }







        /// <summary>
        /// Create NEW by show a CreateWizard dialog, where user HAVE TO SELECT A RELATIVE OBJECT to INITIALIZE ViewModel, then SUBMIT the ViewModel
        /// </summary>
        /// <returns></returns>
        [AccessLevelAuthorize]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult CreateWizard()
        {
            if (!this.isCreateWizard) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            return View();
        }

        /// <summary>
        /// The SUBMITTED ViewModel will be pass to EDIT VIEW to SHOW for editing data
        /// </summary>
        /// <param name="simpleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult CreateWizard(TSimpleViewModel simpleViewModel)
        {
            if (!this.isCreateWizard) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            return View("Edit", this.TailorViewModel(this.DecorateViewModel(simpleViewModel)));
        }







        [AccessLevelAuthorize(GlobalEnums.AccessLevel.Readable)]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Edit(int? id)
        {
            TEntity entity = this.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Readable);
            if (entity == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(this.GetViewModel(entity));
        }


        /// <summary>
        /// Use SubmitTypeOption to DISTINGUISH two type of submit:
        ///     1.SubmitTypeOption.Save: Submit by EDIT VIEW to SAVE ViewModel
        ///     2.SubmitTypeOption.Popup: Submit by CreateWizard dialog, where user BE ABLE TO CHANGE A RELATIVE OBJECT to current ViewModel, then SUBMIT the ViewModel (Note on: 07/07/2015: for example: User may want to change the current edited purchase invoice to adapt to another purchase order - THE RELATIVE OBJECT)
        /// </summary>
        /// <param name="simpleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Edit(TSimpleViewModel simpleViewModel)
        {
            if ((simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Save || simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Closed) && this.Save(simpleViewModel))
                return RedirectToAction(simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Save ? "Edit" : simpleViewModel.PrintAfterClosedSubmit ? "Print" : "Index", new { id = simpleViewModel.GetID() });
            else
            {
                if (simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Popup) this.DecorateViewModel(simpleViewModel);
                return View(this.TailorViewModel(simpleViewModel));
            }
        }





        #region Approve/ UnApprove

        [AccessLevelAuthorize(GlobalEnums.AccessLevel.Readable), ImportModelStateFromTempData]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Approve(int? id)
        {
            TEntity entity = this.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Readable);
            if (entity == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TSimpleViewModel simpleViewModel = this.GetViewModel(entity, true);

            if (!simpleViewModel.Approved)
                if (this.GenericService.GetApprovalPermitted(entity.OrganizationalUnitID))
                    simpleViewModel.Approvable = this.GenericService.Approvable(simpleViewModel);
                else //USER DON'T HAVE PERMISSION TO DO
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            if (simpleViewModel.Approved)
                if (this.GenericService.GetUnApprovalPermitted(entity.OrganizationalUnitID))
                    simpleViewModel.UnApprovable = this.GenericService.UnApprovable(simpleViewModel);
                else //USER DON'T HAVE PERMISSION TO DO
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            return View(simpleViewModel);
        }

        [HttpPost, ActionName("Approve")]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult ApproveConfirmed(TSimpleViewModel simpleViewModel)
        {
            try
            {
                if (this.GenericService.ToggleApproved(simpleViewModel))
                    return RedirectToAction("Index");
                else
                    throw new System.ArgumentException("Lỗi duyệt dữ liệu", "Dữ liệu này không thể duyệt được.");
            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return RedirectToAction("Approve", simpleViewModel.GetID());
            }
        }


        #endregion Approve/ UnApprove





        [AccessLevelAuthorize, ImportModelStateFromTempData]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Delete(int? id)
        {
            TEntity entity = this.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Editable);
            if (entity == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(this.GetViewModel(entity, true));
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (this.GenericService.Delete(id))
                    return RedirectToAction("Index");
                else
                    throw new System.ArgumentException("Lỗi xóa dữ liệu", "Dữ liệu này không thể xóa được.");

            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return RedirectToAction("Delete", id);
            }
        }







        [AccessLevelAuthorize, ImportModelStateFromTempData]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Alter(int? id)
        {
            TEntity entity = this.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Editable);
            if (entity == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(this.GetViewModel(entity, false, true));
        }


        [HttpPost, ActionName("Alter")]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult AlterConfirmed(TSimpleViewModel simpleViewModel)
        {
            try
            {
                if (this.GenericService.Alter(simpleViewModel))
                    return RedirectToAction("Index");
                else
                    throw new System.ArgumentException("Lỗi vô hiệu dữ liệu", "Dữ liệu này không thể vô hiệu.");

            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return View("Alter", this.TailorViewModel(simpleViewModel, false, true));
                //return RedirectToAction("Alter", simpleViewModel.GetID());
            }
        }








        [AccessLevelAuthorize, ImportModelStateFromTempData]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Void(int? id)
        {
            TEntity entity = this.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Editable);
            if (entity == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(this.GetViewModel(entity));
        }


        [HttpPost, ActionName("Void")]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult VoidConfirmed(int id, bool inActive)
        {
            try
            {
                if (this.GenericService.Void(id, inActive))
                    return RedirectToAction("Index");
                else
                    throw new System.ArgumentException("Lỗi vô hiệu dữ liệu", "Dữ liệu này không thể vô hiệu.");

            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return RedirectToAction("Void", id);
            }
        }








        protected virtual TEntity GetEntityAndCheckAccessLevel(int? id, GlobalEnums.AccessLevel accessLevel)
        {
            TEntity entity;
            if (id == null || (entity = this.GenericService.GetByID((int)id)) == null) return null;

            if (this.GenericService.GetAccessLevel(entity.OrganizationalUnitID) < accessLevel) return null;

            return entity;
        }



        protected virtual bool Save(TSimpleViewModel simpleViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return false;//Check Viewmodel IsValid

                TDto dto = simpleViewModel;// Mapper.Map<TSimpleViewModel, TDto>(simpleViewModel);//Convert from Viewmodel to DTO

                if (!this.TryValidateModel(dto)) return false;//Check DTO IsValid
                else
                    if (this.GenericService.Save(dto))
                    {
                        simpleViewModel.SetID(dto.GetID());
                        return true;
                    }
                    else
                        return false;

            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return false;
            }
        }












        protected virtual TSimpleViewModel MapEntityToViewModel(TEntity entity)
        {
            TSimpleViewModel simpleViewModel = Mapper.Map<TSimpleViewModel>(entity);

            return simpleViewModel;
        }

        protected virtual TSimpleViewModel DecorateViewModel(TSimpleViewModel simpleViewModel)
        {
            return simpleViewModel;
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel)
        {
            return this.TailorViewModel(simpleViewModel, false);
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel, bool forDelete)
        {
            return this.TailorViewModel(simpleViewModel, forDelete, false);
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel, bool forDelete, bool forAlter)
        {

            if (!forDelete)//Be caution: the value of simpleViewModel.Editable should be SET EVERY TIME THE simpleViewModel LOADED! This means: if it HAVEN'T SET YET, the default value of simpleViewModel.Editable is FALSE               (THE CONDITIONAL CLAUSE: if (!forDelete) MEAN: WHEN SHOW VIEW FOR DELETE, NO NEED TO CHECK Editable => Editable SHOULD BE FALSE)
                simpleViewModel.Editable = this.GenericService.Editable(simpleViewModel);

            if (forDelete || simpleViewModel is ServiceContractViewModel)//WHEN forDelete, IT SHOULD BE CHECK FOR Deletable ATTRIBUTE, SURELY.          BUT, WHEN OPEN VIEW FOR EDIT, NOW: ONLY VIEW ServiceContract NEED TO USE Deletable ATTRIBUTE ONLY. SO, THIS CODE IS CORRECT FOR NOW, BUT LATER, IF THERE IS MORE VIEWS NEED THIS Deletable ATTRIBUTE, THIS CODE SHOULD MODIFY MORE GENERIC!!!
                simpleViewModel.Deletable = this.GenericService.Deletable(simpleViewModel);

            if (forAlter)//NOW THIS GlobalLocked attribute ONLY be considered WHEN ALTER ACTION to USE IN ALTER VIEW: to ALLOW or NOT ALTER.
                simpleViewModel.GlobalLocked = this.GenericService.GlobalLocked(simpleViewModel);

            RequireJsOptions.Add("Editable", simpleViewModel.Editable, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("Deletable", simpleViewModel.Deletable, RequireJsOptionsScope.Page);

            simpleViewModel.UserID = this.GenericService.UserID; //CAU LENH NAY TAM THOI DUOC SU DUNG DE SORT USER DROPDWONLIST. SAU NAY NEN LAM CACH KHAC, CACH NAY KHONG HAY

            this.viewModelSelectListBuilder.BuildSelectLists(simpleViewModel); //Buil select list for dropdown box using IEnumerable<SelectListItem> (using for short data list only). For the long list, it should use Kendo automplete instead.

            return simpleViewModel;
        }




        #region GetViewModel
        /// <summary>
        /// There are serveral times have to build ViewModel from Entity, by the same steps
        /// This is the reason to have GetViewModel. This is not for any purpose. This GetViewModel may change or ormit if needed
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private TSimpleViewModel GetViewModel(TEntity entity)
        {
            return this.GetViewModel(entity, false);
        }

        private TSimpleViewModel GetViewModel(TEntity entity, bool forDelete)
        {
            return this.GetViewModel(entity, forDelete, false);
        }

        private TSimpleViewModel GetViewModel(TEntity entity, bool forDelete, bool forAlter)
        {
            return this.TailorViewModel(this.DecorateViewModel(this.MapEntityToViewModel(entity)), forDelete, forAlter);
        }
        #endregion GetViewModel




        public ActionResult Print(int? id)
        {
            return View(InitPrintViewModel(id));
        }

        protected virtual PrintViewModel InitPrintViewModel(int? id)
        {
            PrintViewModel printViewModel = new PrintViewModel() { Id = id != null ? (int)id : 0 };
            return printViewModel;
        }

        //Create/CreateWizard: by Authorize Attribute (Editable)
        //Edit: by Authorize Attribute (Readonly?) -> then: Get Entity by ID (Need to check editable ACCESS for entity) -> Check Ediatable of Entity (by service) -> Add FLAG STATUS for Editable/ Readonly -> Set View Status!
        //Index: by Authorize Attribute (Readonly) -> Then load entity list by permission check
        //Save: Check for Ediable for entity (Should check by servicelayer only?)

    }

}