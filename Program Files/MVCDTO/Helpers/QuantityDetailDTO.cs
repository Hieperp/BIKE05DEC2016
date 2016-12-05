using System.ComponentModel.DataAnnotations;

using MVCModel;

namespace MVCDTO.Helpers
{
    public interface IQuantityDetailDTO : IBaseModel
    {
        int CommodityID { get; set; }
        string CommodityCode { get; set; }
        string CommodityName { get; set; }
        int CommodityTypeID { get; set; }

        decimal Quantity { get; set; }
    }

    public abstract class QuantityDetailDTO : BaseModel, IQuantityDetailDTO
    {
        public int CommodityID { get; set; }
        [Display(Name = "Mã hàng")]
        [UIHint("StringReadonly")]
        public string CommodityCode { get; set; }
        [Display(Name = "Hàng hóa")]
        [Required(ErrorMessage = "Vui lòng chọn hàng hóa")]
        //[UIHint("StringReadonly")]
        public virtual string CommodityName { get; set; }

        [Range(1, 99999999999, ErrorMessage = "Lỗi bắt buộc phải có id loại hàng hóa")]
        [Required(ErrorMessage = "Lỗi bắt buộc phải có loại hàng hóa")]
        public int CommodityTypeID { get; set; }


        [Display(Name = "SL")]
        [UIHint("DecimalWithMinus")]
        [Range(0, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public virtual decimal Quantity { get; set; }
    }
}
