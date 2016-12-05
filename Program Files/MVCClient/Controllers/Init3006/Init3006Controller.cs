using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using MVCCore.Repositories.PurchaseTasks;
using MVCCore.Services.PurchaseTasks;
using MVCCore.Services.StockTasks;
using MVCDTO.PurchaseTasks;
using MVCDTO.StockTasks;
using MVCModel.Models;
using MVCClient.Api.CommonTasks;
using MVCCore.Repositories.CommonTasks;
using MVCBase.Enums;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Identity;
using AutoMapper;

namespace MVCClient.Controllers.Init3006
{
    [GenericSimpleAuthorizeAttribute]
    public class Init3006Controller : BaseController
    {
        IPurchaseOrderService purchaseOrderService;
        IPurchaseInvoiceService purchaseInvoiceService;
        IGoodsReceiptService goodsReceiptService;

        IPurchaseOrderRepository purchaseOrderRepository;
        ICommodityRepository commodityRepository;

        IVehicleTransferOrderService transferOrderService;

        public Init3006Controller(IPurchaseOrderService purchaseOrderService, IPurchaseInvoiceService purchaseInvoiceService, IGoodsReceiptService goodsReceiptService, IPurchaseOrderRepository purchaseOrderRepository, ICommodityRepository commodityRepository, IVehicleTransferOrderService transferOrderService)
            : base(purchaseOrderService)
        {
            this.purchaseOrderService = purchaseOrderService;
            this.purchaseInvoiceService = purchaseInvoiceService;
            this.goodsReceiptService = goodsReceiptService;

            this.purchaseOrderRepository = purchaseOrderRepository;
            this.commodityRepository = commodityRepository;

            this.transferOrderService = transferOrderService;
        }



        //Import
        public ActionResult Import()
        {
            return View();
            return View();
            return View();



            //////this.purchaseInvoiceService.UserID = this.purchaseOrderService.UserID;
            //////this.goodsReceiptService.UserID = this.purchaseOrderService.UserID;

            //////PurchaseOrderDTO purchaseOrderDTO = null;
            //////CommoditiesApiController commoditiesApi = new CommoditiesApiController(this.commodityRepository);

            //////int warehouseID = 0; int commodityCategoryID = 0; int countRow = 0;

            //////var purchaseOrderEntity = this.purchaseOrderRepository.GetEntity();
            //////if (purchaseOrderEntity != null) return View(); //Check to exit if not empty

            //////ICollection<Inventory_30_06_33> Inventory_30_06_33s = this.purchaseOrderRepository.GetEntities<Inventory_30_06_33>().OrderBy(o => o.WarehouseID).ThenBy(ob => ob.SupplierID).ToList();
            //////foreach (Inventory_30_06_33 Inventory_30_06_33 in Inventory_30_06_33s)
            //////{
            //////    countRow = countRow + 1;

            //////    if (warehouseID != (int)Inventory_30_06_33.WarehouseID || commodityCategoryID != (int)Inventory_30_06_33.SupplierID || countRow > 55)
            //////    {
            //////        if (purchaseOrderDTO != null) this.SaveInit3006(purchaseOrderDTO, warehouseID);

            //////        commodityCategoryID = (int)Inventory_30_06_33.SupplierID;
            //////        warehouseID = (int)Inventory_30_06_33.WarehouseID;
            //////        countRow = 0;

            //////        purchaseOrderDTO = new PurchaseOrderDTO();
            //////        purchaseOrderDTO.EntryDate = new DateTime(2015, 06, 30);
            //////        purchaseOrderDTO.SupplierID = 1;
            //////        purchaseOrderDTO.PriceTermID = 1;
            //////        purchaseOrderDTO.PaymentTermID = 1;
            //////        purchaseOrderDTO.PreparedPersonID = this.purchaseOrderService.UserID;
            //////        purchaseOrderDTO.ApproverID = this.purchaseOrderService.UserID;
            //////    }


            //////    PurchaseOrderDetailDTO purchaseOrderDetailDTO = new PurchaseOrderDetailDTO();
            //////    purchaseOrderDetailDTO.CommodityID = Inventory_30_06_33.CommodityID;
            //////    purchaseOrderDetailDTO.CommodityCode = Inventory_30_06_33.Description;
            //////    purchaseOrderDetailDTO.CommodityName = Inventory_30_06_33.DescriptionOfficial;
            //////    purchaseOrderDetailDTO.CommodityTypeID = (int)Inventory_30_06_33.ItemTypeID;

            //////    purchaseOrderDetailDTO.ChassisCode = Inventory_30_06_33.SerialNo.Trim() == "" ? null : Inventory_30_06_33.SerialNo.Trim();
            //////    purchaseOrderDetailDTO.EngineCode = Inventory_30_06_33.FixedAsset.Trim() == "" ? null : Inventory_30_06_33.FixedAsset.Trim();
            //////    purchaseOrderDetailDTO.ColorCode = Inventory_30_06_33.ColorCode.Trim() == "" ? null : Inventory_30_06_33.ColorCode.Trim();

            //////    purchaseOrderDetailDTO.Origin = Inventory_30_06_33.WHInputID.ToString();
            //////    purchaseOrderDetailDTO.Packing = Inventory_30_06_33.WHInputDate.ToString();


            //////    purchaseOrderDetailDTO.Quantity = (decimal)Inventory_30_06_33.QuantityEnd;
            //////    purchaseOrderDetailDTO.UnitPrice = (decimal)Inventory_30_06_33.UPriceNMDInventory;
            //////    purchaseOrderDetailDTO.VATPercent = 10;
            //////    purchaseOrderDetailDTO.GrossPrice = Math.Round(purchaseOrderDetailDTO.UnitPrice * (1 + purchaseOrderDetailDTO.VATPercent / 100), 0);

            //////    purchaseOrderDetailDTO.Amount = Math.Round(purchaseOrderDetailDTO.Quantity * purchaseOrderDetailDTO.UnitPrice, 0);
            //////    purchaseOrderDetailDTO.GrossAmount = Math.Round(purchaseOrderDetailDTO.Quantity * purchaseOrderDetailDTO.GrossPrice, 0);
            //////    purchaseOrderDetailDTO.VATAmount = Math.Round(purchaseOrderDetailDTO.GrossAmount - purchaseOrderDetailDTO.Amount, 0);

            //////    purchaseOrderDetailDTO.QuantityInvoice = 0;

            //////    purchaseOrderDTO.TotalQuantity = Math.Round(purchaseOrderDTO.TotalQuantity + purchaseOrderDetailDTO.Quantity, 0);
            //////    purchaseOrderDTO.TotalAmount = Math.Round(purchaseOrderDTO.TotalAmount + purchaseOrderDetailDTO.Amount, 0);
            //////    purchaseOrderDTO.TotalVATAmount = Math.Round(purchaseOrderDTO.TotalVATAmount + purchaseOrderDetailDTO.VATAmount, 0);
            //////    purchaseOrderDTO.TotalGrossAmount = Math.Round(purchaseOrderDTO.TotalGrossAmount + purchaseOrderDetailDTO.GrossAmount, 0);

            //////    purchaseOrderDTO.GetDetails().Add(purchaseOrderDetailDTO);

            //////}

            //////if (purchaseOrderDTO != null) this.SaveInit3006(purchaseOrderDTO, warehouseID);


            return View();
        }



        //Import
        public ActionResult Import072015()
        {

            return View();
            return View();
            return View();


            //////this.purchaseInvoiceService.UserID = this.purchaseOrderService.UserID;
            //////this.goodsReceiptService.UserID = this.purchaseOrderService.UserID;

            //////PurchaseOrderDTO purchaseOrderDTO = null;
            //////CommoditiesApiController commoditiesApi = new CommoditiesApiController(this.commodityRepository);

            //////string loHANG = ""; int countRow = 0; int warehouseID = 0;


            //////ICollection<RAWDATA_07_2015> RAWDATA_07_2015s = this.purchaseOrderRepository.GetEntities<RAWDATA_07_2015>().Where(w => w.WarehouseID == this.purchaseOrderService.LocationID).OrderBy(o => o.LOHANG).ToList();
            //////foreach (RAWDATA_07_2015 RAWDATA_07_2015 in RAWDATA_07_2015s)
            //////{
            //////    countRow = countRow + 1;

            //////    if (loHANG != RAWDATA_07_2015.LOHANG || countRow > 30)
            //////    {
            //////        if (purchaseOrderDTO != null) this.SaveInit3006(purchaseOrderDTO, warehouseID); //Luu y: Moi lan RUN cai foreach nay: la chi load THE SAME WarehouseID, VI VAY, CAI RAWDATA_07_2015.WarehouseID LUON GIONG NHAU

            //////        loHANG = RAWDATA_07_2015.LOHANG;
            //////        countRow = 0;

            //////        purchaseOrderDTO = new PurchaseOrderDTO();
            //////        purchaseOrderDTO.EntryDate = RAWDATA_07_2015.NGAYGIAO;
            //////        purchaseOrderDTO.SupplierID = 1;
            //////        purchaseOrderDTO.PriceTermID = 1;
            //////        purchaseOrderDTO.PaymentTermID = 1;
            //////        purchaseOrderDTO.PreparedPersonID = this.purchaseOrderService.UserID;
            //////        purchaseOrderDTO.ApproverID = this.purchaseOrderService.UserID;

            //////        purchaseOrderDTO.ConfirmReference = RAWDATA_07_2015.LOHANG + RAWDATA_07_2015.LOHANG != RAWDATA_07_2015.DONHANG ? " [" + RAWDATA_07_2015.DONHANG + "]" : "";
            //////    }


            //////    PurchaseOrderDetailDTO purchaseOrderDetailDTO = new PurchaseOrderDetailDTO();
            //////    warehouseID = RAWDATA_07_2015.WarehouseID;
            //////    purchaseOrderDetailDTO.CommodityID = (int)RAWDATA_07_2015.CommodityID;
            //////    purchaseOrderDetailDTO.CommodityCode = RAWDATA_07_2015.MAHANG;
            //////    purchaseOrderDetailDTO.CommodityName = RAWDATA_07_2015.TEN_VN;
            //////    purchaseOrderDetailDTO.CommodityTypeID = (int)RAWDATA_07_2015.CommodityTypeID;

            //////    purchaseOrderDetailDTO.ChassisCode = RAWDATA_07_2015.SOKHUNG == null ? null : (RAWDATA_07_2015.SOKHUNG.Trim() == "" ? null : RAWDATA_07_2015.SOKHUNG.Trim());
            //////    purchaseOrderDetailDTO.EngineCode = RAWDATA_07_2015.SOMAY == null ? null : (RAWDATA_07_2015.SOMAY.Trim() == "" ? null : RAWDATA_07_2015.SOMAY.Trim());
            //////    purchaseOrderDetailDTO.ColorCode = RAWDATA_07_2015.MAUXE == null ? null : (RAWDATA_07_2015.MAUXE.Trim() == "" ? null : RAWDATA_07_2015.MAUXE.Trim());

            //////    purchaseOrderDetailDTO.Quantity = (decimal)RAWDATA_07_2015.SOLUONG;
            //////    purchaseOrderDetailDTO.UnitPrice = (decimal)RAWDATA_07_2015.DONGIA;
            //////    purchaseOrderDetailDTO.VATPercent = 10;
            //////    purchaseOrderDetailDTO.GrossPrice = Math.Round(purchaseOrderDetailDTO.UnitPrice * (1 + purchaseOrderDetailDTO.VATPercent / 100), 0);

            //////    purchaseOrderDetailDTO.Amount = Math.Round(purchaseOrderDetailDTO.Quantity * purchaseOrderDetailDTO.UnitPrice, 0);
            //////    purchaseOrderDetailDTO.GrossAmount = Math.Round(purchaseOrderDetailDTO.Quantity * purchaseOrderDetailDTO.GrossPrice, 0);
            //////    purchaseOrderDetailDTO.VATAmount = Math.Round(purchaseOrderDetailDTO.GrossAmount - purchaseOrderDetailDTO.Amount, 0);

            //////    purchaseOrderDetailDTO.QuantityInvoice = 0;

            //////    purchaseOrderDTO.TotalQuantity = Math.Round(purchaseOrderDTO.TotalQuantity + purchaseOrderDetailDTO.Quantity, 0);
            //////    purchaseOrderDTO.TotalAmount = Math.Round(purchaseOrderDTO.TotalAmount + purchaseOrderDetailDTO.Amount, 0);
            //////    purchaseOrderDTO.TotalVATAmount = Math.Round(purchaseOrderDTO.TotalVATAmount + purchaseOrderDetailDTO.VATAmount, 0);
            //////    purchaseOrderDTO.TotalGrossAmount = Math.Round(purchaseOrderDTO.TotalGrossAmount + purchaseOrderDetailDTO.GrossAmount, 0);

            //////    purchaseOrderDTO.GetDetails().Add(purchaseOrderDetailDTO);

            //////}

            //////if (purchaseOrderDTO != null) this.SaveInit3006(purchaseOrderDTO, warehouseID);


            return View();
        }


        private void SaveInit3006(PurchaseOrderDTO purchaseOrderDTO, int warehouseID)
        {
            if (this.purchaseOrderService.Save(purchaseOrderDTO))
            {
                PurchaseInvoiceDTO purchaseInvoiceDTO = new PurchaseInvoiceDTO();

                purchaseInvoiceDTO.EntryDate = purchaseOrderDTO.EntryDate;

                purchaseInvoiceDTO.SupplierID = purchaseOrderDTO.SupplierID;
                purchaseInvoiceDTO.PurchaseOrderID = purchaseOrderDTO.PurchaseOrderID;

                purchaseInvoiceDTO.PriceTermID = purchaseOrderDTO.PriceTermID;
                purchaseInvoiceDTO.PaymentTermID = purchaseOrderDTO.PaymentTermID;

                purchaseInvoiceDTO.PreparedPersonID = purchaseOrderDTO.PreparedPersonID;
                purchaseInvoiceDTO.ApproverID = purchaseOrderDTO.ApproverID;


                ICollection<PurchaseInvoiceViewDetail> purchaseInvoiceViewDetails = this.purchaseInvoiceService.GetPurchaseInvoiceViewDetails(purchaseInvoiceDTO.PurchaseInvoiceID, purchaseInvoiceDTO.PurchaseOrderID == null ? 0 : (int)purchaseInvoiceDTO.PurchaseOrderID, purchaseInvoiceDTO.SupplierID, false);

                Mapper.Map<ICollection<PurchaseInvoiceViewDetail>, ICollection<PurchaseInvoiceDetailDTO>>(purchaseInvoiceViewDetails, purchaseInvoiceDTO.ViewDetails);


                if (purchaseInvoiceDTO.GetDetails() != null && purchaseInvoiceDTO.GetDetails().Count > 0)
                    purchaseInvoiceDTO.GetDetails().Each(detailDTO =>
                    {
                        detailDTO.Quantity = detailDTO.QuantityRemains;

                        detailDTO.Amount = Math.Round(detailDTO.Quantity * detailDTO.UnitPrice, 0);
                        detailDTO.GrossAmount = Math.Round(detailDTO.Quantity * detailDTO.GrossPrice, 0);
                        detailDTO.VATAmount = Math.Round(detailDTO.GrossAmount - detailDTO.Amount, 0);

                        //detailDTO.QuantityReceipt = 0;

                        purchaseInvoiceDTO.TotalQuantity = Math.Round(purchaseInvoiceDTO.TotalQuantity + detailDTO.Quantity, 0);
                        purchaseInvoiceDTO.TotalAmount = Math.Round(purchaseInvoiceDTO.TotalAmount + detailDTO.Amount, 0);
                        purchaseInvoiceDTO.TotalVATAmount = Math.Round(purchaseInvoiceDTO.TotalVATAmount + detailDTO.VATAmount, 0);
                        purchaseInvoiceDTO.TotalGrossAmount = Math.Round(purchaseInvoiceDTO.TotalGrossAmount + detailDTO.GrossAmount, 0);
                    });

                if (this.purchaseInvoiceService.Save(purchaseInvoiceDTO))
                {
                    GoodsReceiptDTO goodsReceiptDTO = new GoodsReceiptDTO();

                    goodsReceiptDTO.EntryDate = purchaseInvoiceDTO.EntryDate;

                    goodsReceiptDTO.VoucherID = purchaseInvoiceDTO.PurchaseInvoiceID;
                    goodsReceiptDTO.GoodsReceiptTypeID = (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice;

                    goodsReceiptDTO.PreparedPersonID = purchaseInvoiceDTO.PreparedPersonID;
                    goodsReceiptDTO.ApproverID = purchaseInvoiceDTO.ApproverID;

                    ICollection<GoodsReceiptViewDetail> goodsReceiptViewDetails = this.goodsReceiptService.GetGoodsReceiptViewDetails(goodsReceiptDTO.GoodsReceiptID, goodsReceiptDTO.GoodsReceiptTypeID, goodsReceiptDTO.VoucherID, false);

                    Mapper.Map<ICollection<GoodsReceiptViewDetail>, ICollection<GoodsReceiptDetailDTO>>(goodsReceiptViewDetails, goodsReceiptDTO.ViewDetails);


                    if (goodsReceiptDTO.GetDetails() != null && goodsReceiptDTO.GetDetails().Count > 0)
                        goodsReceiptDTO.GetDetails().Each(detailDTO =>
                        {

                            detailDTO.Quantity = detailDTO.QuantityRemains;

                            detailDTO.Amount = Math.Round(detailDTO.Quantity * detailDTO.UnitPrice, 0);
                            detailDTO.GrossAmount = Math.Round(detailDTO.Quantity * detailDTO.GrossPrice, 0);
                            detailDTO.VATAmount = Math.Round(detailDTO.GrossAmount - detailDTO.Amount, 0);

                            //detailDTO.QuantityIssue = 0;

                            goodsReceiptDTO.TotalQuantity = Math.Round(goodsReceiptDTO.TotalQuantity + detailDTO.Quantity, 0);
                            goodsReceiptDTO.TotalAmount = Math.Round(goodsReceiptDTO.TotalAmount + detailDTO.Amount, 0);
                            goodsReceiptDTO.TotalVATAmount = Math.Round(goodsReceiptDTO.TotalVATAmount + detailDTO.VATAmount, 0);
                            goodsReceiptDTO.TotalGrossAmount = Math.Round(goodsReceiptDTO.TotalGrossAmount + detailDTO.GrossAmount, 0);


                            detailDTO.WarehouseID = warehouseID;
                        });
                    this.goodsReceiptService.Save(goodsReceiptDTO);
                }
            }

        }




        public ActionResult ImportVCNB()
        {
            return View("Import");
            return View("Import");
            return View("Import");


            ////////FUNCTION IMPORT NAY HIEN CHUA CHAC DUNG! LY DO: DA TACH TransferOrder THANH VehicleTransferOrder VA PartTransferOrder.
            ////////NEU CAN SU DUNG: PHAI XEM XET LAI THAT KY
            //////this.transferOrderService.UserID = this.purchaseOrderService.UserID;

            //////VehicleTransferOrderDTO transferOrderDTO = null;

            //////int sourceLocationID = 0; int commodityTypeID = 0; int destinationWarehouseID = 0; DateTime ngayGIAO = DateTime.Today; int countRow = 0;

            //////ICollection<TransferOrder> transferOrders = this.purchaseOrderRepository.GetEntities<TransferOrder>();
            //////if (transferOrders != null && transferOrders.Count > 0) return View("Import"); //Check to exit if not empty

            //////ICollection<VCNB_T07> vcnb_T07s = this.purchaseOrderRepository.GetEntities<VCNB_T07>().OrderBy(od => od.NGAYGIAO).ThenBy(o => o.SourceLocationID).ThenBy(ot => ot.CommodityTypeID).ThenBy(ob => ob.DestinationWarehouseID).ToList();
            //////foreach (VCNB_T07 vcnb_T07 in vcnb_T07s)
            //////{
            //////    countRow = countRow + 1;

            //////    if (sourceLocationID != (int)vcnb_T07.SourceLocationID || commodityTypeID != (int)vcnb_T07.CommodityTypeID || destinationWarehouseID != (int)vcnb_T07.DestinationWarehouseID || ngayGIAO != (DateTime)vcnb_T07.NGAYGIAO || countRow > 61)
            //////    {
            //////        if (transferOrderDTO != null) this.transferOrderService.Save(transferOrderDTO);

            //////        commodityTypeID = (int)vcnb_T07.CommodityTypeID;
            //////        sourceLocationID = (int)vcnb_T07.SourceLocationID;
            //////        destinationWarehouseID = (int)vcnb_T07.DestinationWarehouseID;
            //////        ngayGIAO = (DateTime)vcnb_T07.NGAYGIAO;

            //////        countRow = 0;

            //////        transferOrderDTO = new VehicleTransferOrderDTO();
            //////        transferOrderDTO.EntryDate = ngayGIAO;
            //////        transferOrderDTO.SourceLocationID = sourceLocationID;
            //////        transferOrderDTO.WarehouseID = destinationWarehouseID;
            //////        transferOrderDTO.PreparedPersonID = this.purchaseOrderService.UserID;
            //////        transferOrderDTO.ApproverID = this.purchaseOrderService.UserID;
            //////    }


            //////    VehicleTransferOrderDetailDTO vehicleTransferOrderDetailDTO = new VehicleTransferOrderDetailDTO();
            //////    vehicleTransferOrderDetailDTO.CommodityID = (int)vcnb_T07.CommodityID;
            //////    vehicleTransferOrderDetailDTO.CommodityCode = vcnb_T07.MAHANG;
            //////    vehicleTransferOrderDetailDTO.CommodityName = vcnb_T07.TEN_VN;
            //////    vehicleTransferOrderDetailDTO.CommodityTypeID = (int)vcnb_T07.CommodityTypeID;

            //////    vehicleTransferOrderDetailDTO.WarehouseID = (int)vcnb_T07.WarehouseID;

            //////    vehicleTransferOrderDetailDTO.Remarks = (vcnb_T07.SOKHUNG.Trim() == "" ? null : vcnb_T07.SOKHUNG.Trim()) + "#" + (vcnb_T07.SOMAY.Trim() == "" ? null : vcnb_T07.SOMAY.Trim());


            //////    vehicleTransferOrderDetailDTO.Quantity = (decimal)vcnb_T07.SL;
            //////    vehicleTransferOrderDetailDTO.QuantityTransfer = 0;

            //////    transferOrderDTO.TotalQuantity = Math.Round(transferOrderDTO.TotalQuantity + vehicleTransferOrderDetailDTO.Quantity, 0);

            //////    transferOrderDTO.GetDetails().Add(vehicleTransferOrderDetailDTO);

            //////}

            //////if (transferOrderDTO != null) this.transferOrderService.Save(transferOrderDTO);


            return View("Import");
        }


        public ActionResult InitWarehouseBalance15AUG()
        {
            return View("Import");
            return View("Import");


            this.transferOrderService.UserID = this.purchaseOrderService.UserID;
            this.transferOrderService.InitWarehouseBalance15AUG();

            return View("Import");
        }




    }
}