using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCBase.Enums;
using MVCDTO.Helpers;

namespace MVCDTO.SalesTasks
{
    public abstract class SalesInvoicePrimitiveDTO : DiscountVATAmountDTO<SalesInvoiceDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.SalesInvoice; } }

        public int GetID() { return this.SalesInvoiceID; }
        public void SetID(int id) { this.SalesInvoiceID = id; }

        public int SalesInvoiceID { get; set; }

        public int CustomerID { get; set; }
        [Display(Name = "Khách hàng")]
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string CustomerName { get; set; }
        [Display(Name = "Ngày sinh")]
        public Nullable<System.DateTime> CustomerBirthday { get; set; }
        [Display(Name = "Mã số thuế")]
        public string CustomerVATCode { get; set; }
        [Display(Name = "Điện thoại")]
        public string CustomerTelephone { get; set; }
        [Display(Name = "Địa chỉ")]
        public string CustomerAddressNo { get; set; }
        [Display(Name = "Khu vực")]
        public string CustomerEntireTerritoryEntireName { get; set; }

        [Display(Name = "Loại hóa đơn")]
        public virtual int SalesInvoiceTypeID { get; set; }
        [Display(Name = "Phương thức thanh toán")]
        public int PaymentTermID { get; set; }

        public int ReceiptID { get; set; }


        public Nullable<int> PromotionID { get; set; }
        [Display(Name = "Chương trình khuyến mãi")]
        public string PromotionCode { get; set; }
        [Display(Name = "Chứng từ khuyến mãi")]
        public string PromotionVouchers { get; set; }


        public int EmployeeID { get; set; }
        [Display(Name = "Nhân viên thực hiện")]
        [Required(ErrorMessage = "Vui lòng nhập tên nhân viên thực hiện")]
        public string EmployeeName { get; set; }


        public bool IsFinished { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.SalesInvoiceTypeID = this.SalesInvoiceTypeID; e.CustomerID = this.CustomerID; e.PromotionID = this.PromotionID; e.EmployeeID = this.EmployeeID; });
        }

    }

    public class ContractibleInvoicePrimitiveDTO : SalesInvoicePrimitiveDTO //this class should abstract to prevent instantiation, BUT then it can not be used with kendo HTML extention: DisplayNameTitle(). SO WE OMIT MODIFIER: abstract 
    {
        public Nullable<int> ServiceContractID { get; set; }
        public string ServiceContractReference { get; set; }

        public string ServiceContractCommodityID { get; set; }
        [Display(Name = "Mã hàng")]
        public string ServiceContractCommodityCode { get; set; }
        [Display(Name = "Tên hàng")]
        public string ServiceContractCommodityName { get; set; }

        [Display(Name = "Ngày mua")]
        public Nullable<System.DateTime> ServiceContractPurchaseDate { get; set; }
        [Display(Name = "Biển số xe")]
        public string ServiceContractLicensePlate { get; set; }
        [Display(Name = "Số khung")]
        public string ServiceContractChassisCode { get; set; }
        [Display(Name = "Số máy")]
        public string ServiceContractEngineCode { get; set; }
        [Display(Name = "Mã màu")]
        public string ServiceContractColorCode { get; set; }
        [Display(Name = "Tên đại lý")]
        public string ServiceContractAgentName { get; set; }


        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.ServiceContractID = this.ServiceContractID; });
        }
    }

    public class QuotationalInvoicePrimitiveDTO : ContractibleInvoicePrimitiveDTO //this class should abstract to prevent instantiation, BUT then it can not be used with kendo HTML extention: DisplayNameTitle(). SO WE OMIT MODIFIER: abstract 
    {
        public Nullable<int> QuotationID { get; set; }
        [Display(Name = "Phiếu báo giá")]
        public string QuotationReference { get; set; }

        [Display(Name = "Ngày báo giá")]
        public Nullable<System.DateTime> QuotationEntryDate { get; set; }
    }













    public class VehiclesInvoicePrimitiveDTO : SalesInvoicePrimitiveDTO
    {
        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.VehiclesInvoice; } }
        public override int SalesInvoiceTypeID { get { return (int)GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice; } }

        [Display(Name = "Số hóa đơn")]
        [Required(ErrorMessage = "Vui lòng nhập Số hóa đơn")]
        public string VATInvoiceNo { get; set; }
        [Display(Name = "Số seri")]
        [Required(ErrorMessage = "Vui lòng nhập Số seri")]
        public string VATInvoiceSeries { get; set; }
        [Display(Name = "Ngày hóa đơn")]
        [Required(ErrorMessage = "Vui lòng nhập Ngày hóa đơn")]
        public Nullable<System.DateTime> VATInvoiceDate { get; set; }

    }

    public class VehiclesInvoiceDTO : VehiclesInvoicePrimitiveDTO, IBaseDetailEntity<VehiclesInvoiceDetailDTO>
    {
        public VehiclesInvoiceDTO()
        {
            this.VehiclesInvoiceViewDetails = new List<VehiclesInvoiceDetailDTO>();
        }


        public List<VehiclesInvoiceDetailDTO> VehiclesInvoiceViewDetails { get; set; }
        public List<VehiclesInvoiceDetailDTO> ViewDetails { get { return this.VehiclesInvoiceViewDetails; } set { this.VehiclesInvoiceViewDetails = value; } }

        public ICollection<VehiclesInvoiceDetailDTO> GetDetails() { return this.VehiclesInvoiceViewDetails; }

        protected override IEnumerable<SalesInvoiceDetailDTO> DtoDetails() { return this.VehiclesInvoiceViewDetails; }
    }














    public class PartsInvoicePrimitiveDTO : QuotationalInvoicePrimitiveDTO
    {
        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PartsInvoice; } }
        public override int SalesInvoiceTypeID { get { return (int)GlobalEnums.SalesInvoiceTypeID.PartsInvoice; } }

        public Nullable<int> ServiceInvoiceID { get; set; }
        public string ServiceInvoiceReference { get; set; }
        [Display(Name = "Xe đang sửa chữa")]
        public Nullable<System.DateTime> ServiceInvoiceEntryDate { get; set; }
    }

    public class PartsInvoiceDTO : PartsInvoicePrimitiveDTO, IBaseDetailEntity<PartsInvoiceDetailDTO>
    {
        public PartsInvoiceDTO()
        {
            this.PartsInvoiceViewDetails = new List<PartsInvoiceDetailDTO>();
        }


        public List<PartsInvoiceDetailDTO> PartsInvoiceViewDetails { get; set; }
        public List<PartsInvoiceDetailDTO> ViewDetails { get { return this.PartsInvoiceViewDetails; } set { this.PartsInvoiceViewDetails = value; } }

        public ICollection<PartsInvoiceDetailDTO> GetDetails() { return this.PartsInvoiceViewDetails; }

        protected override IEnumerable<SalesInvoiceDetailDTO> DtoDetails() { return this.PartsInvoiceViewDetails; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.GetDetails().ToList().ForEach(e => { e.ServiceInvoiceID = this.ServiceInvoiceID; });
        }
    }














    public class ServicesInvoicePrimitiveDTO : QuotationalInvoicePrimitiveDTO
    {
        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ServicesInvoice; } }
        public override int SalesInvoiceTypeID { get { return (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice; } }

        [Required(ErrorMessage = "Nhập tên nhân viên tiếp nhận")]
        public int ReceptionistID { get; set; }
        [Display(Name = "Nhân viên tiếp nhận")]
        [Required(ErrorMessage = "Nhập tên nhân viên tiếp nhận")]
        public string ReceptionistName { get; set; }

        [Display(Name = "Giờ bắt đầu sửa chữa")]
        //[Required(ErrorMessage = "Nhập ngày giờ sửa chữa")]
        public Nullable<System.DateTime> RespondedDate { get; set; }

        public int ServiceLineID { get; set; }
        [Display(Name = "Số km công tơ mét")]
        public int CurrentMeters { get; set; }

        [Display(Name = "Yêu cầu của khách hàng")]
        public string Damages { get; set; }
        [Display(Name = "Kết quả chuẩn đoán")]
        public string Causes { get; set; }
        [Display(Name = "Biện pháp giải quyết")]
        public string Solutions { get; set; }
        [Display(Name = "Yêu cầu sửa chữa lần tới")]
        public string NextMaintenance { get; set; }

        [Display(Name = "Sửa chữa lớn")]
        public bool IsMajorRepair { get; set; }        
    }

    public class ServicesInvoiceDTO : ServicesInvoicePrimitiveDTO, IBaseDetailEntity<ServicesInvoiceDetailDTO>
    {
        public ServicesInvoiceDTO()
        {
            this.SalesInvoiceDetails = new List<ServicesInvoiceDetailDTO>();
        }


        public List<ServicesInvoiceDetailDTO> SalesInvoiceDetails { get; set; }

        public ICollection<ServicesInvoiceDetailDTO> GetDetails() { return this.SalesInvoiceDetails; }

        protected override IEnumerable<SalesInvoiceDetailDTO> DtoDetails() { return this.SalesInvoiceDetails; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.GetDetails().ToList().ForEach(e => { e.CurrentMeters = this.CurrentMeters; });
        }
    }


}
