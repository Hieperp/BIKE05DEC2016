using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCModel
{
    public interface IBaseModel : IValidatableObject
    {
        DateTime? EntryDate { get; set; }
        int LocationID { get; set; }
    }

    public abstract class BaseModel : IBaseModel
    {
        protected BaseModel() { this.EntryDate = DateTime.Now; }


        [Display(Name = "Ngày lập")]
        [Required(ErrorMessage = "Vui lòng nhập ngày lập")]
        public DateTime? EntryDate { get; set; }

        public int LocationID { get; set; }

        [Display(Name = "Ghi chú")]
        public string Remarks { get; set; }

        #region Implementation of IValidatableObject

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (false) yield return new ValidationResult("", new[] { "" });
        }

        #endregion
    }
}
