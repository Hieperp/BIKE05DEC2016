using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using AutoMapper;

using MVCBase.Enums;
using MVCModel.Models;
using MVCDTO.SalesTasks;
using MVCCore.Repositories.SalesTasks;
using MVCCore.Repositories.CommonTasks;
using MVCCore.Services.SalesTasks;
using MVCCore.Services.Helpers;
using MVCService.Helpers;

namespace MVCService.SalesTasks
{
    public class VehiclesInvoiceService : GenericWithViewDetailService<SalesInvoice, SalesInvoiceDetail, VehiclesInvoiceViewDetail, VehiclesInvoiceDTO, VehiclesInvoicePrimitiveDTO, VehiclesInvoiceDetailDTO>, IVehiclesInvoiceService
    {
        private readonly IServiceContractRepository serviceContractRepository;
        private readonly IAccountInvoiceAPIRepository accountInvoiceAPIRepository;

        private readonly IServiceContractService serviceContractService;
        private readonly IAccountInvoiceService accountInvoiceService;

        public VehiclesInvoiceService(IVehiclesInvoiceRepository vehiclesInvoiceRepository,
                                      IServiceContractRepository serviceContractRepository,
                                      IAccountInvoiceAPIRepository accountInvoiceAPIRepository,
                                      IServiceContractService serviceContractService,
                                      IAccountInvoiceService accountInvoiceService)
            : base(vehiclesInvoiceRepository, "VehiclesInvoicePostSaveValidate", "VehiclesInvoiceSaveRelative", null, "GetVehiclesInvoiceViewDetails")
        {
            this.serviceContractRepository = serviceContractRepository;
            this.accountInvoiceAPIRepository = accountInvoiceAPIRepository;

            this.serviceContractService = serviceContractService;
            this.accountInvoiceService = accountInvoiceService;
        }

        public override ICollection<VehiclesInvoiceViewDetail> GetViewDetails(int salesInvoiceID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("SalesInvoiceID", salesInvoiceID) };
            return this.GetViewDetails(parameters);
        }


        public override bool Save(VehiclesInvoiceDTO vehiclesInvoiceDTO)
        {
            vehiclesInvoiceDTO.VehiclesInvoiceViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(vehiclesInvoiceDTO);
        }

        protected override SalesInvoice SaveMe(VehiclesInvoiceDTO vehiclesInvoiceDTO)
        {
            SalesInvoice salesInvoice = base.SaveMe(vehiclesInvoiceDTO);

            if (salesInvoice.IsFinished)
            {
                ICollection<ServiceContractGetVehiclesInvoice> serviceContractGetVehiclesInvoices = this.serviceContractRepository.ServiceContractGetVehiclesInvoice(salesInvoice.LocationID, "", salesInvoice.SalesInvoiceID, null);
                foreach (ServiceContractGetVehiclesInvoice serviceContractGetVehiclesInvoice in serviceContractGetVehiclesInvoices)
                {

                    ServiceContractDTO serviceContractDTO = new ServiceContractDTO();

                    serviceContractDTO.EntryDate = serviceContractGetVehiclesInvoice.EntryDate;
                    serviceContractDTO.SalesInvoiceDetailID = serviceContractGetVehiclesInvoice.SalesInvoiceDetailID;
                    serviceContractDTO.PurchaseDate = serviceContractGetVehiclesInvoice.EntryDate;

                    serviceContractDTO.ServiceContractTypeID = (int)GlobalEnums.ServiceContractTypeID.Warranty;

                    serviceContractDTO.CustomerID = serviceContractGetVehiclesInvoice.CustomerID;
                    serviceContractDTO.CommodityID = serviceContractGetVehiclesInvoice.CommodityID;
                    serviceContractDTO.ChassisCode = serviceContractGetVehiclesInvoice.ChassisCode;
                    serviceContractDTO.EngineCode = serviceContractGetVehiclesInvoice.EngineCode;
                    serviceContractDTO.ColorCode = serviceContractGetVehiclesInvoice.ColorCode;

                    serviceContractDTO.BeginningDate = serviceContractGetVehiclesInvoice.BeginningDate;
                    serviceContractDTO.EndingDate = serviceContractGetVehiclesInvoice.EndingDate;

                    serviceContractDTO.BeginningMeters = 0;
                    serviceContractDTO.EndingMeters = serviceContractGetVehiclesInvoice.LimitedKilometreWarranty;

                    serviceContractDTO.PreparedPersonID = vehiclesInvoiceDTO.PreparedPersonID;
                    serviceContractDTO.ApproverID = vehiclesInvoiceDTO.ApproverID;

                    this.serviceContractService.UserID = this.UserID; //THE BaseService.UserID IS AUTOMATICALLY SET BY CustomControllerAttribute OF CONTROLLER, ONLY WHEN BaseService IS INITIALIZED BY CONTROLLER. BUT HERE, THE this.serviceContractService IS INITIALIZED BY VehiclesInvoiceService => SO SHOULD SET serviceContractService.UserID = this.UserID
                    this.serviceContractService.Save(serviceContractDTO, true);
                }


                if (vehiclesInvoiceDTO.VATInvoiceNo != null && vehiclesInvoiceDTO.VATInvoiceNo.Trim() != "")
                {
                    AccountInvoiceDTO accountInvoiceDTO = new AccountInvoiceDTO();

                    accountInvoiceDTO.EntryDate = vehiclesInvoiceDTO.EntryDate;
                    accountInvoiceDTO.CustomerID = vehiclesInvoiceDTO.CustomerID;

                    accountInvoiceDTO.VATInvoiceNo = vehiclesInvoiceDTO.VATInvoiceNo;
                    accountInvoiceDTO.VATInvoiceSeries = vehiclesInvoiceDTO.VATInvoiceSeries;
                    accountInvoiceDTO.VATInvoiceDate = vehiclesInvoiceDTO.VATInvoiceDate;

                    accountInvoiceDTO.PreparedPersonID = vehiclesInvoiceDTO.PreparedPersonID;
                    accountInvoiceDTO.ApproverID = vehiclesInvoiceDTO.ApproverID;

                    accountInvoiceDTO.TotalQuantity = vehiclesInvoiceDTO.TotalQuantity;
                    accountInvoiceDTO.TotalAmount = vehiclesInvoiceDTO.TotalAmount;
                    accountInvoiceDTO.TotalVATAmount = vehiclesInvoiceDTO.TotalVATAmount;
                    accountInvoiceDTO.TotalGrossAmount = vehiclesInvoiceDTO.TotalGrossAmount;
                    accountInvoiceDTO.AverageDiscountPercent = vehiclesInvoiceDTO.AverageDiscountPercent;

                    accountInvoiceDTO.Remarks = vehiclesInvoiceDTO.Remarks;

                    string lDescription = "";
                    IEnumerable<PendingSalesInvoice> pendingSalesInvoices = this.accountInvoiceAPIRepository.GetPendingSalesInvoices(salesInvoice.SalesInvoiceID, "", salesInvoice.LocationID, 0, DateTime.Now, DateTime.Now, 0, "");
                    foreach (PendingSalesInvoice pendingSalesInvoice in pendingSalesInvoices)
                    {
                        if (lDescription.IndexOf(pendingSalesInvoice.CommodityName) < 0) lDescription = lDescription + (lDescription != "" ? ", " : "") + pendingSalesInvoice.CommodityName;
                        accountInvoiceDTO.ViewDetails.Add(new AccountInvoiceDetailDTO { SalesInvoiceDetailID = pendingSalesInvoice.SalesInvoiceDetailID, CommodityID = pendingSalesInvoice.CommodityID, CommodityTypeID = pendingSalesInvoice.CommodityTypeID, Quantity = pendingSalesInvoice.Quantity, ListedPrice = pendingSalesInvoice.ListedPrice, DiscountPercent = pendingSalesInvoice.DiscountPercent, UnitPrice = pendingSalesInvoice.UnitPrice, VATPercent = pendingSalesInvoice.VATPercent, GrossPrice = pendingSalesInvoice.GrossPrice, Amount = pendingSalesInvoice.Amount, VATAmount = pendingSalesInvoice.VATAmount, GrossAmount = pendingSalesInvoice.GrossAmount, IsBonus = pendingSalesInvoice.IsBonus, IsWarrantyClaim = pendingSalesInvoice.IsWarrantyClaim });
                    }

                    accountInvoiceDTO.Description = lDescription.Length > 99 ? lDescription.Substring(0, 98) : lDescription;

                    this.accountInvoiceService.UserID = this.UserID; //THE BaseService.UserID IS AUTOMATICALLY SET BY CustomControllerAttribute OF CONTROLLER, ONLY WHEN BaseService IS INITIALIZED BY CONTROLLER. BUT HERE, THE this.accountInvoiceService IS INITIALIZED BY VehiclesInvoiceService => SO SHOULD SET accountInvoiceService.UserID = this.UserID
                    this.accountInvoiceService.Save(accountInvoiceDTO, true);
                }
            }

            return salesInvoice;
        }
    }












    public class PartsInvoiceService : GenericWithViewDetailService<SalesInvoice, SalesInvoiceDetail, PartsInvoiceViewDetail, PartsInvoiceDTO, PartsInvoicePrimitiveDTO, PartsInvoiceDetailDTO>, IPartsInvoiceService
    {
        private DateTime? checkedDate; //For check over stock
        private string warehouseIDList = "";
        private string commodityIDList = "";

        private readonly IInventoryRepository inventoryRepository;
        private readonly IPartsInvoiceHelperService partsInvoiceHelperService;


        public PartsInvoiceService(IPartsInvoiceRepository partsInvoiceRepository, IInventoryRepository inventoryRepository, IPartsInvoiceHelperService partsInvoiceHelperService)
            : base(partsInvoiceRepository, "PartsInvoicePostSaveValidate", "PartsInvoiceSaveRelative", null, "GetPartsInvoiceViewDetails")
        {
            this.inventoryRepository = inventoryRepository;
            this.partsInvoiceHelperService = partsInvoiceHelperService;
        }

        public override ICollection<PartsInvoiceViewDetail> GetViewDetails(int salesInvoiceID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("SalesInvoiceID", salesInvoiceID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(PartsInvoiceDTO partsInvoiceDTO)
        {
            partsInvoiceDTO.PartsInvoiceViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(partsInvoiceDTO);
        }

        protected override void UpdateDetail(PartsInvoiceDTO dto, SalesInvoice entity)
        {
            this.partsInvoiceHelperService.GetWCParameters(dto, null, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UpdateDetail(dto, entity);
        }

        protected override void UndoDetail(PartsInvoiceDTO dto, SalesInvoice entity, bool isDelete)
        {
            this.partsInvoiceHelperService.GetWCParameters(null, entity, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UndoDetail(dto, entity, isDelete);
        }



        protected override void PostSaveValidate(SalesInvoice entity)
        {
            this.inventoryRepository.CheckOverStock(this.checkedDate, this.warehouseIDList, this.commodityIDList);
            base.PostSaveValidate(entity);
        }


        public override bool Editable(PartsInvoiceDTO dto)
        {
            bool editable = base.Editable(dto);
            if (editable && dto.GetID() <= 0 && dto.ServiceInvoiceID != null) //RE CHECK EDITABLE IF THIS IS NEW PartsInvoice for AN EXISTING ServiceInvoice (MEANS: dto.GetID() <= 0 AND dto.ServiceInvoiceID != null)
                return !base.GenericWithDetailRepository.CheckExisting((int)dto.ServiceInvoiceID, "ServicesInvoiceEditable");
            else
                return editable;
        }


    }












    public class ServicesInvoiceService : GenericWithDetailService<SalesInvoice, SalesInvoiceDetail, ServicesInvoiceDTO, ServicesInvoicePrimitiveDTO, ServicesInvoiceDetailDTO>, IServicesInvoiceService
    {
        public ServicesInvoiceService(IServicesInvoiceRepository ServicesInvoiceRepository)
            : base(ServicesInvoiceRepository, "ServicesInvoicePostSaveValidate")
        {
        }

        protected override bool TryValidateModel(ServicesInvoiceDTO dto, ref System.Text.StringBuilder invalidMessage)
        {
            if (dto.IsFinished && dto.RespondedDate == null) invalidMessage.Append(" Vui lòng nhập ngày giờ bắt đầu sửa chữa;");

            return base.TryValidateModel(dto, ref invalidMessage);
        }

        public override bool Save(ServicesInvoiceDTO servicesInvoiceDTO)
        {
            servicesInvoiceDTO.SalesInvoiceDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(servicesInvoiceDTO);
        }
    }





    public class PartsInvoiceHelperService : HelperService<SalesInvoice, SalesInvoiceDetail, PartsInvoiceDTO, PartsInvoiceDetailDTO>, IPartsInvoiceHelperService
    {
    }


}
