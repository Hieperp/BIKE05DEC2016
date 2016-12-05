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
    public class QuotationPrimitiveDTO : DiscountVATAmountDTO<QuotationDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Quotation; } }

        public int GetID() { return this.QuotationID; }
        public void SetID(int id) { this.QuotationID = id; }

        public int QuotationID { get; set; }

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

        [Display(Name = "Phương thức thanh toán")]
        public int PaymentTermID { get; set; }



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


        public int EmployeeID { get; set; }
        [Display(Name = "Nhân viên thực hiện")]
        [Required(ErrorMessage = "Vui lòng nhập tên nhân viên thực hiện")]
        public string EmployeeName { get; set; }

        
        public string Damages { get; set; }
        public string Causes { get; set; }
        public string Solutions { get; set; }

        public bool IsFinished { get; set; }
    }


    public class QuotationDTO : QuotationPrimitiveDTO, IBaseDetailEntity<QuotationDetailDTO>
    {
        public QuotationDTO()
        {
            this.QuotationViewDetails = new List<QuotationDetailDTO>();
        }


        public List<QuotationDetailDTO> QuotationViewDetails { get; set; }
        public List<QuotationDetailDTO> ViewDetails { get { return this.QuotationViewDetails; } set { this.QuotationViewDetails = value; } }

        public ICollection<QuotationDetailDTO> GetDetails() { return this.QuotationViewDetails; }

        protected override IEnumerable<QuotationDetailDTO> DtoDetails() { return this.QuotationViewDetails; }
    }


}
