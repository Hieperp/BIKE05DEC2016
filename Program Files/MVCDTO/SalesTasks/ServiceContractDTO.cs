using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCBase.Enums;

namespace MVCDTO.SalesTasks
{
    public class ServiceContractPrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ServiceContract; } }

        public int GetID() { return this.ServiceContractID; }
        public void SetID(int id) { this.ServiceContractID = id; }

        public int ServiceContractID { get; set; }

        public int CustomerID { get; set; }
        [Display(Name = "Khách hàng")]
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
        

        public int ServiceContractTypeID { get; set; }
        public Nullable<int> SalesInvoiceDetailID { get; set; }

        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        [Display(Name = "Hàng hóa")]
        [Required(ErrorMessage = "Vui lòng nhập tên xe")]
        public string CommodityName { get; set; }

        [Display(Name = "Ngày mua")]
        public Nullable<System.DateTime> PurchaseDate { get; set; }

        [Display(Name = "Biển số xe")]
        public string LicensePlate { get; set; }
        [Display(Name = "Số khung")]
        public string ChassisCode { get; set; }
        [Display(Name = "Số máy")]
        public string EngineCode { get; set; }
        [Display(Name = "Mã màu")]
        public string ColorCode { get; set; }
        [Display(Name = "Tên đại lý")]
        public string AgentName { get; set; }


        [Display(Name = "KM bắt đầu")]
        public Nullable<int> BeginningMeters { get; set; }
        [Display(Name = "KM kết thúc")]
        public Nullable<int> EndingMeters { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        public Nullable<System.DateTime> BeginningDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public Nullable<System.DateTime> EndingDate { get; set; }

    }

    public class ServiceContractDTO : ServiceContractPrimitiveDTO
    {
        public ServiceContractDTO()
        {

        }
    }
}
