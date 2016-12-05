using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCBase.Enums;

namespace MVCDTO.CommonTasks
{

    public class CustomerPrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Customer; } }

        public int GetID() { return this.CustomerID; }
        public void SetID(int id) { this.CustomerID = id; }

        public int CustomerID { get; set; }
        [Required]
        [Display(Name = "Tên khách")]
        public string Name { get; set; }
        [Display(Name = "Tên đầy đủ")]
        public string OfficialName { get; set; }
        [Display(Name = "Phân khúc khách hàng")]
        [DefaultValue(1)]
        public int CustomerCategoryID { get; set; }
        [Display(Name = "Phân loại khách hàng")]
        [DefaultValue(1)]
        public int CustomerTypeID { get; set; }
        [Display(Name = "Khu vực")]
        public int TerritoryID { get; set; }
        [Display(Name = "Khu vực")]
        [Required]
        public string EntireTerritoryEntireName { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required]
        public string AddressNo { get; set; }
        [Display(Name = "Mã số thuế")]
        public string VATCode { get; set; }
        [Display(Name = "Điện thoại")]
        [Required]
        public string Telephone { get; set; }
        public string Facsimile { get; set; }
        [Display(Name = "Người liên hệ")]
        public string AttentionName { get; set; }
        [Display(Name = "Chức danh")]
        public string AttentionTitle { get; set; }
        [Required]
        [Display(Name = "Ngày sinh")]
        public Nullable<System.DateTime> Birthday { get; set; }
        [Display(Name = "Hạn mức tín dụng")]
        public Nullable<double> LimitAmount { get; set; }

        [Display(Name = "Khách hàng")]
        public bool IsCustomer { get; set; }
        [Display(Name = "Nhà cung cấp")]
        public bool IsSupplier { get; set; }
        [Display(Name = "Giới tính nữ")]
        public bool IsFemale { get; set; }

        public override int PreparedPersonID { get { return 1; } }
    }

    public class CustomerDTO : CustomerPrimitiveDTO
    {
    }
}
